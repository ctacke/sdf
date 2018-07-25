using OpenNETCF.Configuration;

namespace OpenNETCF.Diagnostics
{
    class SwitchesDictionarySectionHandler : DictionarySectionHandler
    {
        public SwitchesDictionarySectionHandler() { }

        protected override string KeyAttributeName { get { return "name"; } }
        internal override bool ValueRequired { get { return true; } }
    }
}
