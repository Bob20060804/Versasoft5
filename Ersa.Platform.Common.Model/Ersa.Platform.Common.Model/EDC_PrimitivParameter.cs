using Ersa.Platform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ersa.Platform.Common.Model
{
    /// <summary>
    /// 原始参数
    /// </summary>
	public class EDC_PrimitivParameter : EDC_AdressRelevanterTeil, ICloneable
    {
        /// <summary>
        /// 值
        /// </summary>
		private object m_objValue;

        /// <summary>
        /// 显示值
        /// </summary>
		private object m_objAnzeigeWert;

        /// <summary>
        /// 参数说明
        /// </summary>
        public EDC_ParameterBeschreibung PRO_edcParameterBeschreibung { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public ENUM_ParameterTypen PRO_enmParameterTyp { get; set; }

        /// <summary>
        /// 单位键
        /// </summary>
        public string PRO_strEinheitKey { get; set; }

        /// <summary>
        /// 实际地址
        /// </summary>
        public string PRO_strPhysischeAdresse { get; set; }

        public virtual object PRO_objValue
        {
            get
            {
                return m_objValue;
            }
            set
            {
                if (m_objValue != value)
                {
                    m_objValue = value;
                    EDC_Dispatch.SUB_AktionStarten(delegate
                    {
                        RaisePropertyChanged("PRO_objValue");
                        RaisePropertyChanged("PRO_objAnzeigeWert");
                    });
                }
            }
        }

        public virtual object PRO_objAnzeigeWert
        {
            get
            {
                return m_objAnzeigeWert ?? m_objValue;
            }
            set
            {
                SetProperty(ref m_objAnzeigeWert, value, "PRO_objAnzeigeWert");
            }
        }

        public virtual bool PRO_blnHatAenderung => !object.Equals(PRO_objValue, PRO_objAnzeigeWert);

        public override IEnumerable<EDC_AdressRelevanterTeil> FUN_enuElementeHolen()
        {
            return new EDC_PrimitivParameter[1]
            {
                this
            };
        }

        public void SUB_AnzeigeWertUebernehmen()
        {
            PRO_objValue = PRO_objAnzeigeWert;
            PRO_objAnzeigeWert = null;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("EDC_PrimitivParameter: ");
            if (!string.IsNullOrWhiteSpace(base.PRO_strAdresse))
            {
                stringBuilder.Append("PRO_strAdresse: " + base.PRO_strAdresse);
            }
            else
            {
                stringBuilder.Append("PRO_strAdresse(NULL)");
            }
            if (PRO_objValue != null)
            {
                stringBuilder.Append(" PRO_objValue=" + PRO_objValue);
            }
            else
            {
                stringBuilder.Append(" PRO_objValue(NULL)");
            }
            if (PRO_objAnzeigeWert != null)
            {
                stringBuilder.Append(" PRO_objAnzeigeWert=" + PRO_objAnzeigeWert);
            }
            else
            {
                stringBuilder.Append(" PRO_objAnzeigeWert(NULL)");
            }
            return stringBuilder.ToString();
        }
    }
}
