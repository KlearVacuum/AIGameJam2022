namespace GOAP
{
    public struct Attribute<ValueType>
    {
        private ValueType m_Value;
        public ValueType Value => m_Value;

        private float m_Confidence;
        public float Confidence => m_Confidence;

        public void Set(ValueType value) => m_Value = value;
    }
}
