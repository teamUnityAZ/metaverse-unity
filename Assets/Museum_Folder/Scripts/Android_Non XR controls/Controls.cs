//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Museum_Folder/Scripts/Android_Non XR controls/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""dd11df1c-fd1e-4637-9385-0c1094fa8405"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""afea6473-bf8b-4500-bcf3-f92cf8001631"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""44a2a403-cda9-4715-9c09-82469e3b4fa0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""c215eb8c-db8e-497f-b99e-e83a0e71ae74"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""c73e858e-baf4-491e-b9b8-ba62b1916a64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""72fea615-1358-46e8-bfe7-3fe63dd38fc9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Continue"",
                    ""type"": ""Button"",
                    ""id"": ""d5ddd472-6c57-4a91-ad49-28bd6c0d7489"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""58fea5e4-2eef-4bb9-935a-a4da04a999af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""gyro"",
                    ""type"": ""Value"",
                    ""id"": ""c0f3d9ed-2fc6-4ce7-b129-4a31d85828d2"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PrimaryTouchPhase"",
                    ""type"": ""Value"",
                    ""id"": ""9b4b220a-2982-4734-9156-aacc7809591c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SecondaryTouchPhase"",
                    ""type"": ""Value"",
                    ""id"": ""9f5a5a0b-5fe8-46a4-bcd1-dea3ef453264"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PrimaryFingerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""ed03c072-8da9-4352-bbf1-966d52404839"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SecondaryFingerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""fec9d8f9-01f6-42c1-ad4f-2374475f21ae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SecondaryTouchContact"",
                    ""type"": ""Value"",
                    ""id"": ""b67fd5dd-e942-44b8-a459-e622ce6cdeb5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""96b8d9ff-0fb4-4ae3-b4cb-b6071f3a5f01"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""75d3db2d-4c96-4494-b653-58cdbf8b61f2"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""18206b0a-ab4f-4497-954f-6d19b83c99e3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d3d255df-c96a-4038-b654-93e1f6a5f93a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5774db56-5056-4eac-82fa-1eb1abc2c67f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""05d1964a-aa9f-4ee5-944a-83446c415a95"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""edbc6f1f-c66c-4de8-b7dd-9244532c60be"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""85ecec87-cf29-461c-8ccf-5487d704bc6d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7d46608b-5fd3-40fe-9cc0-7af2b8ba756c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""13ddbf95-8865-40fd-a578-224911e459c7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f3b85828-5e19-4b0e-ba2e-df684dbc83a0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e9a9eb7-7adb-444e-a163-f6d5c8dcbf3a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard;UI"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""451aeeca-baeb-4e2e-95db-2f241c21163f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9ab858c-d64e-4f50-9ba0-70b787c492fc"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5826ee15-9e7d-4544-b8e1-9611156c7867"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard;UI"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42e81511-e457-4755-a693-7b8c9c110ab8"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""UI"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ac4eee2-e1c4-408d-9207-05adbf019e95"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38dee37f-de84-41f4-a1c9-4f0874347378"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard;UI"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07584089-afce-46ed-868e-a7ac37c6978d"",
                    ""path"": ""<Gyroscope>/angularVelocity"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""gyro"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ecafc51e-81f4-43aa-a54c-655a0fc7f846"",
                    ""path"": ""<Touchscreen>/touch0/phase"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouchPhase"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c56075ee-5058-4f75-9b97-eb70003bc5a0"",
                    ""path"": ""<Touchscreen>/touch1/phase"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchPhase"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27313465-4690-4919-853b-65665ff27cc4"",
                    ""path"": ""<Touchscreen>/touch0/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryFingerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""717d03ec-2723-42e8-9286-f87831525db4"",
                    ""path"": ""<Touchscreen>/touch1/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryFingerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""525c58d2-8e31-48b2-9cbd-6d84efa14181"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoard"",
            ""bindingGroup"": ""KeyBoard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""bindingGroup"": ""UI"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
        m_Gameplay_Action = m_Gameplay.FindAction("Action", throwIfNotFound: true);
        m_Gameplay_Back = m_Gameplay.FindAction("Back", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Continue = m_Gameplay.FindAction("Continue", throwIfNotFound: true);
        m_Gameplay_Sprint = m_Gameplay.FindAction("Sprint", throwIfNotFound: true);
        m_Gameplay_gyro = m_Gameplay.FindAction("gyro", throwIfNotFound: true);
        m_Gameplay_PrimaryTouchPhase = m_Gameplay.FindAction("PrimaryTouchPhase", throwIfNotFound: true);
        m_Gameplay_SecondaryTouchPhase = m_Gameplay.FindAction("SecondaryTouchPhase", throwIfNotFound: true);
        m_Gameplay_PrimaryFingerPosition = m_Gameplay.FindAction("PrimaryFingerPosition", throwIfNotFound: true);
        m_Gameplay_SecondaryFingerPosition = m_Gameplay.FindAction("SecondaryFingerPosition", throwIfNotFound: true);
        m_Gameplay_SecondaryTouchContact = m_Gameplay.FindAction("SecondaryTouchContact", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Look;
    private readonly InputAction m_Gameplay_Action;
    private readonly InputAction m_Gameplay_Back;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Continue;
    private readonly InputAction m_Gameplay_Sprint;
    private readonly InputAction m_Gameplay_gyro;
    private readonly InputAction m_Gameplay_PrimaryTouchPhase;
    private readonly InputAction m_Gameplay_SecondaryTouchPhase;
    private readonly InputAction m_Gameplay_PrimaryFingerPosition;
    private readonly InputAction m_Gameplay_SecondaryFingerPosition;
    private readonly InputAction m_Gameplay_SecondaryTouchContact;
    public struct GameplayActions
    {
        private @Controls m_Wrapper;
        public GameplayActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Look => m_Wrapper.m_Gameplay_Look;
        public InputAction @Action => m_Wrapper.m_Gameplay_Action;
        public InputAction @Back => m_Wrapper.m_Gameplay_Back;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Continue => m_Wrapper.m_Gameplay_Continue;
        public InputAction @Sprint => m_Wrapper.m_Gameplay_Sprint;
        public InputAction @gyro => m_Wrapper.m_Gameplay_gyro;
        public InputAction @PrimaryTouchPhase => m_Wrapper.m_Gameplay_PrimaryTouchPhase;
        public InputAction @SecondaryTouchPhase => m_Wrapper.m_Gameplay_SecondaryTouchPhase;
        public InputAction @PrimaryFingerPosition => m_Wrapper.m_Gameplay_PrimaryFingerPosition;
        public InputAction @SecondaryFingerPosition => m_Wrapper.m_Gameplay_SecondaryFingerPosition;
        public InputAction @SecondaryTouchContact => m_Wrapper.m_Gameplay_SecondaryTouchContact;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Action.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAction;
                @Back.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBack;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Continue.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnContinue;
                @Continue.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnContinue;
                @Continue.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnContinue;
                @Sprint.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @gyro.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGyro;
                @gyro.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGyro;
                @gyro.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGyro;
                @PrimaryTouchPhase.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryTouchPhase;
                @PrimaryTouchPhase.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryTouchPhase;
                @PrimaryTouchPhase.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryTouchPhase;
                @SecondaryTouchPhase.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryTouchPhase;
                @SecondaryTouchPhase.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryTouchPhase;
                @SecondaryTouchPhase.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryTouchPhase;
                @PrimaryFingerPosition.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryFingerPosition;
                @SecondaryFingerPosition.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryTouchContact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryTouchContact;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Continue.started += instance.OnContinue;
                @Continue.performed += instance.OnContinue;
                @Continue.canceled += instance.OnContinue;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @gyro.started += instance.OnGyro;
                @gyro.performed += instance.OnGyro;
                @gyro.canceled += instance.OnGyro;
                @PrimaryTouchPhase.started += instance.OnPrimaryTouchPhase;
                @PrimaryTouchPhase.performed += instance.OnPrimaryTouchPhase;
                @PrimaryTouchPhase.canceled += instance.OnPrimaryTouchPhase;
                @SecondaryTouchPhase.started += instance.OnSecondaryTouchPhase;
                @SecondaryTouchPhase.performed += instance.OnSecondaryTouchPhase;
                @SecondaryTouchPhase.canceled += instance.OnSecondaryTouchPhase;
                @PrimaryFingerPosition.started += instance.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.performed += instance.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.canceled += instance.OnPrimaryFingerPosition;
                @SecondaryFingerPosition.started += instance.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.performed += instance.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.canceled += instance.OnSecondaryFingerPosition;
                @SecondaryTouchContact.started += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled += instance.OnSecondaryTouchContact;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_KeyBoardSchemeIndex = -1;
    public InputControlScheme KeyBoardScheme
    {
        get
        {
            if (m_KeyBoardSchemeIndex == -1) m_KeyBoardSchemeIndex = asset.FindControlSchemeIndex("KeyBoard");
            return asset.controlSchemes[m_KeyBoardSchemeIndex];
        }
    }
    private int m_UISchemeIndex = -1;
    public InputControlScheme UIScheme
    {
        get
        {
            if (m_UISchemeIndex == -1) m_UISchemeIndex = asset.FindControlSchemeIndex("UI");
            return asset.controlSchemes[m_UISchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnContinue(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnGyro(InputAction.CallbackContext context);
        void OnPrimaryTouchPhase(InputAction.CallbackContext context);
        void OnSecondaryTouchPhase(InputAction.CallbackContext context);
        void OnPrimaryFingerPosition(InputAction.CallbackContext context);
        void OnSecondaryFingerPosition(InputAction.CallbackContext context);
        void OnSecondaryTouchContact(InputAction.CallbackContext context);
    }
}
