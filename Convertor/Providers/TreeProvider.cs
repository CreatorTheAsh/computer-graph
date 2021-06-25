using Converter.Interfaces;
using Converter.OcTree;

namespace Converter.Providers
{
    public class TreeProvider : ITreeProvider
    {
        public ITree GetTree()
        {
            return new Tree();
        }
    }
}