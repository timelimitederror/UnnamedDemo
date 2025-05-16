using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanelController : MonoBehaviour
{
    public SkillStateController skill_1;
    public SkillStateController skill_2;
    public SkillStateController skill_3;
    public SkillStateController skill_4;
    public SkillStateController mixing;
    public ArmStateController arm;

    private Action<PlayerSkillStateChanged> playerSkillStateChangedAction;
    private Action<PlayerSkillKeyPositionChanged> playerSkillKeyPositionChangedAction;
    private Action<PlayerSkillColorChanged> playerSkillColorChangedAction;

    // Start is called before the first frame update
    void Start()
    {
        playerSkillStateChangedAction = new Action<PlayerSkillStateChanged>(changeSkillState);
        playerSkillKeyPositionChangedAction = new Action<PlayerSkillKeyPositionChanged>(changeSkillKeyPosition);
        playerSkillColorChangedAction = new Action<PlayerSkillColorChanged>(changeSkillColor);

        EventBus.Subscribe(playerSkillStateChangedAction);
        EventBus.Subscribe(playerSkillKeyPositionChangedAction);
        EventBus.Subscribe(playerSkillColorChangedAction);

        changeSkillKeyPosition(InitManager.Instance.GetKeyPosition());
    }

    private void changeSkillState(PlayerSkillStateChanged skillEvent)
    {
        skill_1.setState(skillEvent.skill_1);
        skill_2.setState(skillEvent.skill_2);
        skill_3.setState(skillEvent.skill_3);
        skill_4.setState(skillEvent.skill_4);
        mixing.setState(skillEvent.mixing);
        arm.setState(skillEvent.normalAttack);
    }

    private void changeSkillColor(PlayerSkillColorChanged skillEvent)
    {
        arm.setColor(skillEvent.color);
    }

    private void changeSkillKeyPosition(PlayerSkillKeyPositionChanged skillEvent)
    {
        skill_1.setKeyPosition(skillEvent.skill_1);
        skill_2.setKeyPosition(skillEvent.skill_2);
        skill_3.setKeyPosition(skillEvent.skill_3);
        skill_4.setKeyPosition(skillEvent.skill_4);
        mixing.setKeyPosition(skillEvent.mixing);
        arm.setKeyPosition(skillEvent.armColor);
    }
}
