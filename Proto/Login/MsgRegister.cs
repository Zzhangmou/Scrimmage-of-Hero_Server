//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/MsgRegister.proto
namespace proto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgRegister")]
  public partial class MsgRegister : global::ProtoBuf.IExtensible
  {
    public MsgRegister() {}
    
    private string _userName = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"userName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string userName
    {
      get { return _userName; }
      set { _userName = value; }
    }
    private string _id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string id
    {
      get { return _id; }
      set { _id = value; }
    }
    private string _pw;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"pw", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string pw
    {
      get { return _pw; }
      set { _pw = value; }
    }
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}