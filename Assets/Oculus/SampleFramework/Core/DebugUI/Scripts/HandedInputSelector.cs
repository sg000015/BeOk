/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided �AS IS� WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class HandedInputSelector : MonoBehaviour
{
    OVRCameraRig m_CameraRig;
    OVRInputModule m_InputModule;

    OVRInputModule Es;
    void Start()
    {
        m_CameraRig = FindObjectOfType<OVRCameraRig>();
        m_InputModule = FindObjectOfType<OVRInputModule>();

        Es = transform.Find("EventSystem").GetComponent<OVRInputModule>();

    }

    void Update()
    {
        if (OVRInput.GetActiveController() == OVRInput.Controller.LTouch)
        {
            SetActiveController(OVRInput.Controller.LTouch);
            Es.joyPadClickButton = (OVRInput.Button)0x00006000;
        }
        else
        {
            SetActiveController(OVRInput.Controller.RTouch);
            Es.joyPadClickButton = (OVRInput.Button)0x00600000;
        }

    }

    void SetActiveController(OVRInput.Controller c)
    {
        Transform t;
        if (c == OVRInput.Controller.LTouch)
        {
            t = m_CameraRig.leftHandAnchor;
        }
        else
        {
            t = m_CameraRig.rightHandAnchor;
        }
        m_InputModule.rayTransform = t;
    }
}
