using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayVFX(GameObject effectObject, Vector3 effectPosition)
    {
        GameObject vfxObject = Instantiate(effectObject, effectPosition, Quaternion.identity);
        ParticleSystem[] particleSystem = vfxObject.GetComponentsInChildren<ParticleSystem>();

        float maxLength = 0f;
        foreach (ParticleSystem individualParticleSystem in particleSystem)
        {
            float currentKnownMaxLength = individualParticleSystem.main.duration
                + individualParticleSystem.main.startLifetime.constantMax;

            if (currentKnownMaxLength > maxLength)
                maxLength = currentKnownMaxLength;
        }
        Destroy(vfxObject, maxLength);
    }
}
