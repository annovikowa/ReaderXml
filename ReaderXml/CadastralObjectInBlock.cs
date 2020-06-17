using ReaderXml.Models;

namespace ReaderXml
{
    public class CadastralObjectInBlock : CadastralObject
    {
        protected CadastralBlock _cadastralBlock;

        public string CadasstralBlockNumber => _cadastralBlock?.CadastralNumber ?? "";

        public void SetCadastralBlock(CadastralBlock cadastralBlock) => _cadastralBlock = cadastralBlock;
    }
}
