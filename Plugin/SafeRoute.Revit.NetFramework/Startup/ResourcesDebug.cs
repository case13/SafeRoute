using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Revit.NetFramework.Startup
{
    public static class ResourcesDebug
    {
        public static string DumpGResources()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resName = asm.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(".g.resources"));

            if (resName == null)
                return "Não achou .g.resources";

            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new ResourceReader(stream))
            {
                var items = reader.Cast<System.Collections.DictionaryEntry>()
                                  .Select(e => e.Key.ToString())
                                  .OrderBy(s => s);

                return string.Join("\n", items);
            }
        }
    }
}
