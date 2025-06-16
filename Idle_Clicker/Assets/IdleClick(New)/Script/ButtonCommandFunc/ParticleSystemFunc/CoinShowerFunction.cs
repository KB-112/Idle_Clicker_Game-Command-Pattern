using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker
{
    public class CoinShowerFunction : MonoBehaviour
    {
   
  

       
        public void DeployParticleSystem(ParticleSystemConfig particleSystemInfo)
        {
            particleSystemInfo.coinShower.Play();
        }
    }
    [System.Serializable]
    public class ParticleSystemConfig
    { 

        public string name;
        public ParticleSystem coinShower;



    }

}
