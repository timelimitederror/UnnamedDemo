using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 控制角色移动，跳跃，飞行
public class PlayerController : MonoBehaviour
{
    private float moveOriginalSpeed = 7f;
    private float flySpeedMultiplier = 2f; // 飞行状态速度乘数
    private float flyFallSpeed = -0.2f;
    private float jumpHeight = 2f;
    private float Gravity = -1f; // 重力
    private float moveSpeed = 7f;
    private float dangerouseSpeed = -4f; // 高空掉落扣血
    private CharacterController characterController;
    private Transform cameraTransform;
    private Vector3 cameraForwardNormalize;
    private Vector3 cameraRightNormalize;
    private PlayerAnimationController playerAnimationController;
    private PlayerSpecialEffectController specialEffectController;
    private PlayerStateController playerStateController;
    private Vector3 velocity = Vector3.zero;// 角色当前速度
    private float isGroundedVelocity = -2f;// 在地面时的y轴速度，保证CharacterController.isGrounded()不误判
    private bool isFall = false;
    private bool isFly = false;

    private float stamina = 200f;
    private float maxStamina = 200f;
    private float flyStaminaConsumption = 10f;

    public bool IsFall
    {
        get
        {
            return isFall;
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        playerAnimationController = GetComponent<PlayerAnimationController>();

        specialEffectController = GetComponent<PlayerSpecialEffectController>();

        playerStateController = GetComponent<PlayerStateController>();

        //cameraTransform = transform.Find("FreeLook Camera");
        cameraTransform = Camera.main.transform;

        setCameraInfo();

        updateGravity();

        // 初始化UI
        EventBus.Publish<PlayerStaminaChanged>(new PlayerStaminaChanged(stamina, maxStamina));
    }

    /*
     帧更新：
    更新重力信息
    跳跃功能
    飞行功能
    镜头移动功能
    角色移动功能
    耐力恢复
     
     */
    void Update()
    {
        if (!TimeManager.Instance.timeFlow())
        {
            return;
        }
        updateGravity();
        jump();
        updateCameraInfo();
        move();
        restoreStamina();
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    // // CharacterController碰撞消息函数
    //}

    // 更新相机信息（如果有鼠标移动增量）
    private void updateCameraInfo()
    {
        if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0)
        {
            setCameraInfo();
        }
    }

    // 设置相机信息
    private void setCameraInfo()
    {
        // 获取相机参考系方向（排除Y轴影响）
        cameraForwardNormalize = cameraTransform.forward;
        cameraForwardNormalize.y = 0; // 保持水平移动
        cameraForwardNormalize.Normalize();

        cameraRightNormalize = cameraTransform.right;
        cameraRightNormalize.y = 0; // 保持水平移动
        cameraRightNormalize.Normalize();
    }

    // 控制角色水平移动
    private void move()
    {
        // 移动控制
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ad左右，ws前后，相对相机位置移动
        if (horizontal != 0f || vertical != 0f)
        {
            if (horizontal != 0f)
            {
                horizontal = horizontal > 0 ? 1f : -1f;
            }

            if (vertical != 0f)
            {
                vertical = vertical > 0 ? 1f : -1f;
            }

            // 组合输入方向（相对于相机坐标系）
            //Vector3 dir = (vertical * cameraForward + horizontal * cameraRight).normalized;
            Vector3 dir = (vertical * cameraForwardNormalize + horizontal * cameraRightNormalize).normalized;
            Vector3 movement = dir * moveSpeed * Time.deltaTime;
            characterController.Move(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.15f);
            playerAnimationController.run();
        }
        else
        {
            playerAnimationController.idle();
        }

        // ad转向，ws移动
        //Vector3 move = transform.forward * moveSpeed * vertical;
        //rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        //transform.Rotate(Vector3.up, horizontal * rotateSpeed);


        // 2.5D移动
        //Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;
        //if (dir != Vector3.zero)
        //{
        //    Vector3 movement = dir * moveSpeed;
        //    rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        //}

    }

    // 控制角色跳跃和飞行
    private void jump()
    {
        // 跳跃
        if (Input.GetButtonDown("Jump"))// && isGrounded
        {
            if (characterController.isGrounded)
            {
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * Gravity);
                playerAnimationController.jump();
            }
            else if (!isFly && changeStamina(flyStaminaConsumption)) // 飞行
            {

                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * Gravity);// 向上移动一段
                isFly = true;
                moveSpeed = moveOriginalSpeed * flySpeedMultiplier;// 修改水平移动速度
                playerAnimationController.jump();// 换成飞行动画
                playerAnimationController.startFly();
                specialEffectController.startFly();

            }
            else
            {
                isFly = false;
                playerAnimationController.stopFly();
                specialEffectController.stopFly();
                moveSpeed = moveOriginalSpeed;
                //playerAnimationController.fall();
            }
        }

        if (isFly)
        {
            if (!changeStamina(flyStaminaConsumption * Time.deltaTime))
            {
                isFly = false;
                playerAnimationController.stopFly();
                specialEffectController.stopFly();
                moveSpeed = moveOriginalSpeed;
            }
        }
    }

    // 更新重力信息，包含对腾空落地的检测
    private void updateGravity()
    {
        // 应用重力
        characterController.Move(velocity * Time.fixedDeltaTime);
        // 地面检测
        if (characterController.isGrounded)
        {
            // 腾空落地检查，用来改动画和移动速度
            if (isFall)
            {
                // 高空掉落扣血，根据触地时的y轴速度计算扣血量
                if (!isFly && velocity.y < dangerouseSpeed)
                {
                    int damage = (int)(100f * (velocity.y * 2 / dangerouseSpeed));
                    playerStateController.health = playerStateController.health >= damage ? playerStateController.health - damage : 0;
                    playerStateController.setHealthUI();
                    if (playerStateController.health > 0)
                    {
                        playerStateController.hitSound();
                    }
                }

                isFall = false;
                isFly = false;
                moveSpeed = moveOriginalSpeed;
                playerAnimationController.stopFly();
                specialEffectController.stopFly();
                playerAnimationController.jumpEnd();
            }

            velocity.y = isGroundedVelocity;
        }
        else if (!isFly)// 普通坠落
        {
            // 重力累加
            velocity.y += Gravity * Time.fixedDeltaTime;
            isFall = true;
        }
        else// 飞行下落
        {
            // 飞行下落累加
            velocity.y += flyFallSpeed * Time.fixedDeltaTime;
            isFall = true;
        }
    }

    private bool changeStamina(float value)
    {// 消耗体力
        if (stamina - value < 0)
        {
            return false;
        }
        stamina -= value;
        EventBus.Publish<PlayerStaminaChanged>(new PlayerStaminaChanged(stamina, maxStamina));
        return true;
    }

    private void restoreStamina()
    {// 在地面时恢复体力
        if (stamina < maxStamina && !isFall)
        {
            stamina += 5f * Time.fixedDeltaTime;
            EventBus.Publish<PlayerStaminaChanged>(new PlayerStaminaChanged(stamina, maxStamina));
        }
    }

    public void ChangeVeolity(float xValue, float yValue, float zValue)
    {
        velocity.x += xValue;
        velocity.y += yValue;
        velocity.z += zValue;
    }
}
