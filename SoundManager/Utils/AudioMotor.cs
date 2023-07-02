using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioLowPassFilter))]
    public class AudioMotor : MonoBehaviour
    {
        public AudioClip motorClip;
        public float motorMinimumPitch = 0.2f;
        public float motorIdlePitch = 0.5f;
        public float motorMaxPitch = 1.5f;
        public float motorPitchChangeReactivity = 1f;
        public float motorDefaultVolume = 0.6f;
        public float motorThrustVolume = 1f;
        public float motorThrustChangeReactivity = 10f;

        [SerializeField]
        AudioSource optionnalTiresOnRoad;

        [SerializeField]
        private bool startEngineOnEnable = true;

        private AudioSource audioSource;
        private AudioLowPassFilter lowPassFilter;
        private float actualTargetSpeed;
        private float targetPitch;
        private float targetThrust;
        private bool isEngineStarted = false;

        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            lowPassFilter = GetComponent<AudioLowPassFilter>();
            if (startEngineOnEnable)
            {
                StartEngine();
            }
        }

        private void OnDisable()
        {
            StopEngine();
        }

        private void Update()
        {
            if (targetPitch != audioSource.pitch)
            {
                audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * motorPitchChangeReactivity);
                if (optionnalTiresOnRoad != null)
                {
                    optionnalTiresOnRoad.volume = Mathf.Lerp(optionnalTiresOnRoad.volume, actualTargetSpeed, Time.deltaTime * motorPitchChangeReactivity);
                }
            }
            if (targetThrust != audioSource.volume)
            {
                audioSource.volume = Mathf.Lerp(audioSource.volume, targetThrust, Time.deltaTime * motorThrustChangeReactivity);
                lowPassFilter.cutoffFrequency = Mathf.Lerp(7000, 14000, audioSource.volume);
                if (audioSource.volume == 0)
                {
                    audioSource.Stop();
                    if (optionnalTiresOnRoad != null)
                    {
                        optionnalTiresOnRoad.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Start the engine
        /// </summary>
        public void StartEngine()
        {
            if (isEngineStarted) return;
            audioSource.pitch = motorMinimumPitch;
            audioSource.volume = 0;
            targetPitch = motorIdlePitch;
            targetThrust = motorDefaultVolume;
            audioSource.clip = motorClip;
            audioSource.loop = true;
            audioSource.Play();
            if(optionnalTiresOnRoad != null)
            {
                optionnalTiresOnRoad.loop = true;
                optionnalTiresOnRoad.Play();
            }
            isEngineStarted = true;
        }

        /// <summary>
        /// Stop the engine
        /// </summary>
        public void StopEngine()
        {
            if (!isEngineStarted) return;
            actualTargetSpeed = 0;
            targetPitch = motorMinimumPitch;
            targetThrust = 0;
            isEngineStarted = false;
        }

        /// <summary>
        /// Set the target speed of the motor
        /// </summary>
        /// <param name="speedPercentage">The target speed percentage (0-1)</param>
        public void SetTargetSpeed(float speedPercentage)
        {
            if (!isEngineStarted) return;
            actualTargetSpeed = speedPercentage;
            targetPitch = Mathf.Lerp(motorIdlePitch, motorMaxPitch, speedPercentage);
        }

        /// <summary>
        /// Set the target thrust of the motor
        /// </summary>
        /// <param name="thrustPercentage">The target thrust percentage (0-1)</param>
        public void SetTargetThrust(float thrustPercentage)
        {
            if (!isEngineStarted) return;
            targetThrust = Mathf.Lerp(motorDefaultVolume, motorThrustVolume, thrustPercentage);
        }
    }
}