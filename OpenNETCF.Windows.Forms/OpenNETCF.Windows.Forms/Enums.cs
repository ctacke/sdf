using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Windows.Forms
{




    /*internal enum CDS
    {
        ZERO = 0,
        TEST = 0x00000002,
        VIDEOPARAMETERS = 0x00000020,
        RESET = 0x40000000
    }*/

    #region Key State Flags
    /// <summary>
    /// KeyStateFlags for Keyboard methods
    /// </summary>
    [Flags()]
    internal enum KeyStateFlags : int
    {
        /// <summary>
        /// Key is toggled.
        /// </summary>
        Toggled = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        AsyncDown = 0x0002,		//	 went down since last GetAsync call.
        /// <summary>
        /// Key was previously down.
        /// </summary>
        PrevDown = 0x0040,
        /// <summary>
        /// Key is currently down.
        /// </summary>
        Down = 0x0080,
        /// <summary>
        /// Left or right CTRL key is down.
        /// </summary>
        AnyCtrl = 0x40000000,
        /// <summary>
        /// Left or right SHIFT key is down.
        /// </summary>
        AnyShift = 0x20000000,
        /// <summary>
        /// Left or right ALT key is down.
        /// </summary>
        AnyAlt = 0x10000000,
        /// <summary>
        /// VK_CAPITAL is toggled.
        /// </summary>
        Capital = 0x08000000,
        /// <summary>
        /// Left CTRL key is down.
        /// </summary>
        LeftCtrl = 0x04000000,
        /// <summary>
        /// Left SHIFT key is down.
        /// </summary>
        LeftShift = 0x02000000,
        /// <summary>
        /// Left ALT key is down.
        /// </summary>
        LeftAlt = 0x01000000,
        /// <summary>
        /// Left Windows logo key is down.
        /// </summary>
        LeftWin = 0x00800000,
        /// <summary>
        /// Right CTRL key is down.
        /// </summary>
        RightCtrl = 0x00400000,
        /// <summary>
        /// Right SHIFT key is down
        /// </summary>
        RightShift = 0x00200000,
        /// <summary>
        /// Right ALT key is down
        /// </summary>
        RightAlt = 0x00100000,
        /// <summary>
        /// Right Windows logo key is down.
        /// </summary>
        RightWin = 0x00080000,
        /// <summary>
        /// Corresponding character is dead character.
        /// </summary>
        Dead = 0x00020000,
        /// <summary>
        /// No characters in pCharacterBuffer to translate.
        /// </summary>
        NoCharacter = 0x00010000,
        /// <summary>
        /// Use for language specific shifts.
        /// </summary>
        Language1 = 0x00008000,
        /// <summary>
        /// NumLock toggled state.
        /// </summary>
        NumLock = 0x00001000,
    }
    #endregion

    internal enum KeyEvents
    {
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        Silent = 0x0004
    }
}
