using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker
{
    public interface IButtonCommand
    {
        void Execute(string name);
    }
}
