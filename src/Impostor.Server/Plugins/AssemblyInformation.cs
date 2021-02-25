using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Impostor.Server.Plugins
{
    public class AssemblyInformation : IAssemblyInformation
    {
        private Assembly _assembly;

        public AssemblyInformation(AssemblyName assemblyName, string path, bool isPlugin)
        {
            AssemblyName = assemblyName;
            Path = path;
            IsPlugin = isPlugin;
        }

        public string Path { get; }

        public bool IsPlugin { get; }

        public AssemblyName AssemblyName { get; }

        public Assembly Load(AssemblyLoadContext context)
        {
            if (_assembly == null)
            {
                _assembly = Assembly.LoadFile(Path);
            }

            return _assembly;
        }
    }
}
