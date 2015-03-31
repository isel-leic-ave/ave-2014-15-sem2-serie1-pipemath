using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PipeMath
{

    public interface Loader
    {
        List<IArrayOperation> LoadOperations();
    }

    public class LoaderOfTypes : Loader
    {
        public LoaderOfTypes(String file)
        {
            throw new NotImplementedException();
        }
        public List<IArrayOperation> LoadOperations()
        {
            throw new NotImplementedException();
        }
    }

    public class LoaderOfMethods : Loader
    {
        public LoaderOfMethods(String file)
        {
            throw new NotImplementedException();
        }

        public List<IArrayOperation> LoadOperations()
        {
            throw new NotImplementedException();
        }
    }

}
