using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.UnitTests.Mocks;

public class MockLeaveTypeRepository
{
    public static Mock<ILeaveTypeRepository> GetMockLeaveTypeRepository()
    {
        var leaveTypes = new List<LeaveType>
        {
            new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vaction",
            }
        };

        var leaveTypeMockRepo = new Mock<ILeaveTypeRepository>();

        leaveTypeMockRepo.Setup(r => r.GetAsync()).ReturnsAsync(leaveTypes);
        leaveTypeMockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>()));

        return leaveTypeMockRepo;
    }
}