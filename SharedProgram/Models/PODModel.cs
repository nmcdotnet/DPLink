using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProgram.Models
{
    public class PODModel
    {
        public PODModel()
        {
            
        }
        public PODModel(int Index, string Value, TypePOD Type, string PODName)
        {
            _Index = Index;
            _Value = Value;
            _Type = Type;
            _PODName = PODName;
        }
        private int _Index;

		public int Index
		{
			get { return _Index; }
			set { _Index = value; }
		}

		private string? _Value;
		public string? Value
		{
			get { return _Value; }
			set { _Value = value; }
		}

		private TypePOD _Type;
		public TypePOD Type
		{
			get { return _Type; }
			set { _Type = value; }
		}

		private string _PODName;
		public string PODName
		{
			get { return _PODName; }
			set { _PODName = value; }
		}

        public override string ToString()
        {
            if (_Type == TypePOD.DATETIME)
            {
                return "<DATETIME>";
            }
            else if (_Type == TypePOD.FIELD)
            {
                return PODName != "" ? $"<{PODName}> ({TypePOD.FIELD.ToString() + _Index})" : $"<{TypePOD.FIELD.ToString() + _Index}>";
            }
            else
            {
                return $"<TEXT>";
            }
        }

        public string ToStringSample()
        {
            if (_Type == TypePOD.DATETIME)
            {
                return "<DATETIME>";
            }
            else if (_Type == TypePOD.FIELD)
            {
                return $"<{PODName}>";
            }
            else
            {
                return $"<{Value}>";
            }
        }

        public PODModel Clone()
        {
            var POD = new PODModel
            {
                Index = Index,
                Value = Value,
                Type = Type,
                PODName = PODName
            };
            return POD;
        }

        public enum TypePOD
        {
            TEXT,
            FIELD,
            DATETIME
        }

    }
}
