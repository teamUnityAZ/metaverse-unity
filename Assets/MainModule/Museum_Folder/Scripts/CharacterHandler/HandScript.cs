using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HandScript : MonoBehaviour
{

    public XRIDefaultInputActions inputActions;
    private Animator _handAnimator;

    public bool isLeftHand;

    private void Awake()
    {

        inputActions = new XRIDefaultInputActions();
        _handAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    float gripValue, triggerValue;
    private void Update()
    {
        if (isLeftHand)
        {
            gripValue = inputActions.XRILeftHand.Select.ReadValue<float>();
            triggerValue = inputActions.XRILeftHand.Activate.ReadValue<float>();
            _handAnimator.SetFloat("Grip", gripValue);
            _handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            gripValue = inputActions.XRIRightHand.Select.ReadValue<float>();
            triggerValue = inputActions.XRIRightHand.Activate.ReadValue<float>();
            _handAnimator.SetFloat("Grip", gripValue);
            _handAnimator.SetFloat("Trigger", triggerValue);

        }
    }




    //    [SerializeField]
    //    private InputActionAsset actionsAssets;
    //    [SerializeField]
    //    private string ControllerName;
    //    [SerializeField] 
    //        private string actionNameTrigger;
    //    [SerializeField]
    //    private string actionNamegrip;

    //    private InputActionMap _actionMap;
    //    private InputAction _inputActionTrigger;
    //    private InputAction _inputActionGrip;

    //    private Animator _handAnimator;



    //    private void Awake()
    //    {
    //        //Getting All the Actions
    //        _actionMap = actionsAssets.FindActionMap(ControllerName);
    //        _inputActionGrip = _actionMap.FindAction(actionNamegrip);
    //        _inputActionTrigger = _actionMap.FindAction(actionNameTrigger);


    //        _handAnimator = GetComponent<Animator>();

    //    }

    //    private void OnEnable()
    //    {
    //        _inputActionGrip.Enable();
    //        _inputActionTrigger.Enable();
    //    }
    //    private void OnDisable()
    //    {
    //        _inputActionGrip.Disable();
    //        _inputActionTrigger.Disable();
    //    }


    //    // Update is called once per frame
    //    void Update()
    //    {
    //        var gripValue = _inputActionGrip.ReadValue<float>();
    //        var triggerValue = _inputActionTrigger.ReadValue<float>();
    //        Debug.Log("gripValue is "+ gripValue);
    //        Debug.Log("triggerValue is " + triggerValue);
    //        _handAnimator.SetFloat("Grip", gripValue);
    //        _handAnimator.SetFloat("Trigger", triggerValue);

    //    }



}
