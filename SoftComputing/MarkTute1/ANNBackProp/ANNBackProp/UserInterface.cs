using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANNShell
{
    // this class is a parent class for user interfaces in the hope it will help
    // allow linux and macos implementations in the future
    public class UserInterface
    {
        public virtual void error(string s)
        {

        }
        public virtual void clear(string s)
        {

        }
        public virtual void warning(string s)
        {

        }
        public virtual void note(string s)
        {

        }
        public virtual void console(string s)
        {

        }
    }
}
