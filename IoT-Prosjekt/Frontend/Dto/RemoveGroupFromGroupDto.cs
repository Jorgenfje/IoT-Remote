using System.Reflection.PortableExecutable;

namespace Frontend.Dto;

public class RemoveGroupFromGroupDto
{
    public int GroupId { get; set; }
    public int DeviceId { get; set; }
}