
namespace ContentCenter.Services
{
    public interface IConvertManager
    {
        string DecodePasswordStructure(byte[] bytes_Input, ref int KeySize, ref byte[] KeyData);
    }
}
