using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ���ƽ�ɫ�ƶ�����Ծ������
public class PlayerController : MonoBehaviour
{
    private float moveOriginalSpeed = 7f;
    private float flySpeedMultiplier = 2f; // ����״̬�ٶȳ���
    private float flyFallSpeed = -0.2f;
    private float jumpHeight = 2f;
    private float Gravity = -1f; // ����
    private float moveSpeed = 7f;
    private float dangerouseSpeed = -4f; // �߿յ����Ѫ
    private CharacterController characterController;
    private Transform cameraTransform;
    private Vector3 cameraForwardNormalize;
    private Vector3 cameraRightNormalize;
    private PlayerAnimationController playerAnimationController;
    private PlayerSpecialEffectController specialEffectController;
    private PlayerStateController playerStateController;
    private Vector3 velocity = Vector3.zero;// ��ɫ��ǰ�ٶ�
    private float isGroundedVelocity = -2f;// �ڵ���ʱ��y���ٶȣ���֤CharacterController.isGrounded()������
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

        // ��ʼ��UI
        EventBus.Publish<PlayerStaminaChanged>(new PlayerStaminaChanged(stamina, maxStamina));
    }

    /*
     ֡���£�
    ����������Ϣ
    ��Ծ����
    ���й���
    ��ͷ�ƶ�����
    ��ɫ�ƶ�����
    �����ָ�
     
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
    // // CharacterController��ײ��Ϣ����
    //}

    // ���������Ϣ�����������ƶ�������
    private void updateCameraInfo()
    {
        if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0)
        {
            setCameraInfo();
        }
    }

    // ���������Ϣ
    private void setCameraInfo()
    {
        // ��ȡ����ο�ϵ�����ų�Y��Ӱ�죩
        cameraForwardNormalize = cameraTransform.forward;
        cameraForwardNormalize.y = 0; // ����ˮƽ�ƶ�
        cameraForwardNormalize.Normalize();

        cameraRightNormalize = cameraTransform.right;
        cameraRightNormalize.y = 0; // ����ˮƽ�ƶ�
        cameraRightNormalize.Normalize();
    }

    // ���ƽ�ɫˮƽ�ƶ�
    private void move()
    {
        // �ƶ�����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ad���ң�wsǰ��������λ���ƶ�
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

            // ������뷽��������������ϵ��
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

        // adת��ws�ƶ�
        //Vector3 move = transform.forward * moveSpeed * vertical;
        //rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        //transform.Rotate(Vector3.up, horizontal * rotateSpeed);


        // 2.5D�ƶ�
        //Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;
        //if (dir != Vector3.zero)
        //{
        //    Vector3 movement = dir * moveSpeed;
        //    rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        //}

    }

    // ���ƽ�ɫ��Ծ�ͷ���
    private void jump()
    {
        // ��Ծ
        if (Input.GetButtonDown("Jump"))// && isGrounded
        {
            if (characterController.isGrounded)
            {
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * Gravity);
                playerAnimationController.jump();
            }
            else if (!isFly && changeStamina(flyStaminaConsumption)) // ����
            {

                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * Gravity);// �����ƶ�һ��
                isFly = true;
                moveSpeed = moveOriginalSpeed * flySpeedMultiplier;// �޸�ˮƽ�ƶ��ٶ�
                playerAnimationController.jump();// ���ɷ��ж���
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

    // ����������Ϣ���������ڿ���صļ��
    private void updateGravity()
    {
        // Ӧ������
        characterController.Move(velocity * Time.fixedDeltaTime);
        // ������
        if (characterController.isGrounded)
        {
            // �ڿ���ؼ�飬�����Ķ������ƶ��ٶ�
            if (isFall)
            {
                // �߿յ����Ѫ�����ݴ���ʱ��y���ٶȼ����Ѫ��
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
        else if (!isFly)// ��ͨ׹��
        {
            // �����ۼ�
            velocity.y += Gravity * Time.fixedDeltaTime;
            isFall = true;
        }
        else// ��������
        {
            // ���������ۼ�
            velocity.y += flyFallSpeed * Time.fixedDeltaTime;
            isFall = true;
        }
    }

    private bool changeStamina(float value)
    {// ��������
        if (stamina - value < 0)
        {
            return false;
        }
        stamina -= value;
        EventBus.Publish<PlayerStaminaChanged>(new PlayerStaminaChanged(stamina, maxStamina));
        return true;
    }

    private void restoreStamina()
    {// �ڵ���ʱ�ָ�����
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
