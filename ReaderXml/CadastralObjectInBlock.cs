using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml
{
    public class CadastralObjectInBlock : CadastralObject
    {
        protected CadastralBlock _cadastralBlock;

        public string CadasstralBlockNumber => _cadastralBlock?.CadastralNumber ?? "";

        public void SetCadastralBlock(CadastralBlock cadastralBlock) => _cadastralBlock = cadastralBlock;
    }
}
