namespace Ordering.Orders.Dtos;

public record AddressDto
(
    string FirstName,
    string LastName,
    string EMailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode
 );

