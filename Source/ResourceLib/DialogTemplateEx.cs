﻿using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Vestris.ResourceLib
{
    /// <summary>
    /// A container for the DIALOGTEMPLATEEX structure.
    /// </summary>
    public class DialogTemplateEx : DialogTemplateBase
    {
        User32.DLGTEMPLATEEX _header = new User32.DLGTEMPLATEEX();
        private byte _characterSet = 0;
        private UInt16 _weight = 0;
        private bool _italic = false;

        /// <summary>
        /// Indicates the character set to use.
        /// </summary>
        public byte CharacterSet
        {
            get
            {
                return _characterSet;
            }
            set
            {
                _characterSet = value;
            }
        }

        /// <summary>
        /// Specifies the x-coordinate, in dialog box units, of the upper-left corner of the dialog box. 
        /// </summary>
        public override Int16 x
        {
            get
            {
                return _header.x;
            }
            set
            {
                _header.x = value;
            }
        }

        /// <summary>
        /// Specifies the y-coordinate, in dialog box units, of the upper-left corner of the dialog box.
        /// </summary>
        public override Int16 y
        {
            get
            {
                return _header.y;
            }
            set
            {
                _header.y = value;
            }
        }

        /// <summary>
        /// Specifies the width, in dialog box units, of the dialog box.
        /// </summary>
        public override Int16 cx
        {
            get
            {
                return _header.cx;
            }
            set
            {
                _header.cx = value;
            }
        }

        /// <summary>
        /// Specifies the height, in dialog box units, of the dialog box.
        /// </summary>
        public override Int16 cy
        {
            get
            {
                return _header.cy;
            }
            set
            {
                _header.cy = value;
            }
        }

        /// <summary>
        /// Dialog style.
        /// </summary>
        public override UInt32 Style
        {
            get
            {
                return _header.style;
            }
            set
            {
                _header.style = value;
            }
        }

        /// <summary>
        /// Extended dialog style.
        /// </summary>
        public override UInt32 ExtendedStyle
        {
            get
            {
                return _header.exStyle;
            }
            set
            {
                _header.exStyle = value;
            }
        }

        /// <summary>
        /// Specifies the weight of the font. 
        /// </summary>
        public UInt16 Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        /// <summary>
        /// Indicates whether the font is italic.
        /// </summary>
        public bool Italic
        {
            get
            {
                return _italic;
            }
            set
            {
                _italic = value;
            }
        }

        /// <summary>
        /// Number of dialog items.
        /// </summary>
        public override UInt16 ControlCount
        {
            get
            {
                return _header.cDlgItems;
            }
        }

        /// <summary>
        /// An extended dialog structure.
        /// </summary>
        public DialogTemplateEx()
        {

        }

        /// <summary>
        /// Read the dialog resource.
        /// </summary>
        /// <param name="lpRes">Pointer to the beginning of the dialog structure.</param>
        internal override IntPtr Read(IntPtr lpRes)
        {
            _header = (User32.DLGTEMPLATEEX)Marshal.PtrToStructure(
                lpRes, typeof(User32.DLGTEMPLATEEX));

            lpRes = base.Read(new IntPtr(lpRes.ToInt32() + Marshal.SizeOf(_header)));

            if ((Style & (uint)User32.DialogStyles.DS_SETFONT) > 0
                || (Style & (uint)User32.DialogStyles.DS_SHELLFONT) > 0)
            {
                // weight
                Weight = (UInt16)Marshal.ReadInt16(lpRes);
                lpRes = new IntPtr(lpRes.ToInt32() + 2);
                // italic
                Italic = (Marshal.ReadByte(lpRes) > 0);
                lpRes = new IntPtr(lpRes.ToInt32() + 1);
                // character set
                CharacterSet = Marshal.ReadByte(lpRes);
                lpRes = new IntPtr(lpRes.ToInt32() + 1);
                // typeface
                TypeFace = Marshal.PtrToStringUni(lpRes);
                lpRes = new IntPtr(lpRes.ToInt32() + (TypeFace.Length + 1) * 2);
            }

            return lpRes;
        }

        internal override IntPtr AddControl(IntPtr lpRes)
        {
            DialogTemplateControlEx control = new DialogTemplateControlEx();
            Controls.Add(control);
            return control.Read(lpRes);
        }

        public override string ToString()
        {
            return string.Format("DIALOGEX {0}", base.ToString());
        }
    }
}