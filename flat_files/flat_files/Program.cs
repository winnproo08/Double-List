
using PlainFile.core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

var dataTypes = new Data("Users.txt", "People.csv", "log.txt");
var csvHelper = new NugetCsvHelper();
var culture = CultureInfo.InvariantCulture;

var users = dataTypes.LoadUsers().ToList();
var people = csvHelper.Read("People.csv").ToList();

User? loggedInUser = null;
int attempts = 0;
do
{
    Console.WriteLine("Usuario: ");
    var username = Console.ReadLine();
    Console.WriteLine("Contraseña: ");
    var password = Console.ReadLine();

    var foundUser = users.FirstOrDefault(userItem => userItem.Username == username);

    if (foundUser != null && foundUser.Active && foundUser.Password == password)
    {
        loggedInUser = foundUser;
        Console.WriteLine($"Bienvenido {foundUser.Username}");
        dataTypes.WriteLog(foundUser.Username, "Login exitoso");
        break;
    }

    attempts++;
    Console.WriteLine("Credenciales incorrectas.");

    if (attempts == 3 && foundUser != null)
    {
        foundUser.Active = false;
        dataTypes.SaveUsers(users);
        Console.WriteLine($"Usuario '{foundUser.Username}' ha sido bloqueado.");
        dataTypes.WriteLog(foundUser.Username, "Usuario bloqueado");
    }


} while (attempts < 3);

if (loggedInUser == null)
{
    Console.WriteLine("No se pudo autenticar. Saliendo...");
    return;
}

var option = string.Empty;
do
{

    option = ShowMenu();
    Console.WriteLine();

    switch (option)
    {
        case "1":
            ShowPeople();
            dataTypes.WriteLog(loggedInUser.Username, "Mostrar personas");
            break;

        case "2":
            AddPerson();
            dataTypes.WriteLog(loggedInUser.Username, "Agregar persona");
            break;

        case "3":
            EditPerson();
            dataTypes.WriteLog(loggedInUser.Username, "Editar persona");
            break;

        case "4":
            DeletePerson();
            dataTypes.WriteLog(loggedInUser.Username, "Eliminar persona");
            break;

        case "5":
            csvHelper.Write("People.csv", people);
            Console.WriteLine("Archivo guardado correctamente.");
            dataTypes.WriteLog(loggedInUser.Username, "Guardar archivo People.csv");
            break;

        case "6":
            ReportByCity();
            dataTypes.WriteLog(loggedInUser.Username, "Reporte por ciudad");
            break;

        case "0":
            Console.WriteLine("Saliendo...");
            dataTypes.WriteLog(loggedInUser.Username, "Salir del sistema");
            break;

        default:
            Console.WriteLine("Opción inválida.");
            break;
    }



} while (option != "0");

void ReportByCity()
{
    var groupedByCity = people
      .OrderBy(p => p.Id)
      .GroupBy(p => string.IsNullOrWhiteSpace(p.City) ? "(Sin ciudad)" : p.City);

    decimal totalGeneral = 0;

    foreach (var cityGroup in groupedByCity)
    {
        Console.WriteLine();
        Console.WriteLine($"Ciudad: {cityGroup.Key}");
        Console.WriteLine();

        Console.WriteLine("ID\tNombres\t\tApellidos\tSaldo");
        Console.WriteLine("—\t——————————\t——————————\t—————————");

        decimal subtotal = 0;

        foreach (var person in cityGroup)
        {
            Console.WriteLine(
                $"{person.Id}\t{person.Name}\t\t{person.LastName}\t{person.Balance.ToString("N2", culture)}"
            );
            subtotal += person.Balance;
        }

        Console.WriteLine("\t\t\t\t=======");
        Console.WriteLine($"Total: {cityGroup.Key}\t\t\t{subtotal.ToString("N2", culture)}");

        totalGeneral += subtotal;
        Console.WriteLine();
    }

    Console.WriteLine("\t\t\t\t=======");
    Console.WriteLine($"Total General:\t\t\t{totalGeneral.ToString("N2", culture)}");
}

void DeletePerson()
{
    Console.Write("Ingresa el ID que deseas eliminar: ");
    if (!int.TryParse(Console.ReadLine(), out int id)) return;
    var personToDelete = people.FirstOrDefault(personItem => personItem.Id == id);
    if (personToDelete == null)
    {
        Console.WriteLine("Persona no encontrada.");
        return;
    }

    Console.WriteLine($"¿Está seguro de eliminar a: {personToDelete.Name} {personToDelete.LastName},  Telefono: {personToDelete.Phone},  Ciudad: {personToDelete.City}, Saldo: {personToDelete.Balance}");
    Console.WriteLine();
    Console.Write("Seleccione una opción: 1.Confirmar o 2.Cancelar ");
    var confirmation = Console.ReadLine()?.ToUpper();
    if (confirmation == "1")
    {
        people.Remove(personToDelete);
        Console.WriteLine("Persona eliminada exitosamente.");
        for (int i = 0; i < people.Count; i++)
        {
            people[i].Id = i + 1;
        }
    }
    else
    {
        Console.WriteLine("Operación cancelada.");
    }


}

void EditPerson()
{
    Console.Write("ID a editar: ");
    if (!int.TryParse(Console.ReadLine(), out int id)) return;

    var personToEdit = people.FirstOrDefault(personItem => personItem.Id == id);
    if (personToEdit == null)
    {
        Console.WriteLine("Persona no encontrada.");
        return;
    }
    Console.WriteLine("Presione ENTER para mantener el valor actual.");

    Console.Write($"Nombres ({personToEdit.Name}): ");
    var nameInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(nameInput)) personToEdit.Name = nameInput;

    Console.Write($"Apellidos ({personToEdit.LastName}): ");
    var lastInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(lastInput)) personToEdit.LastName = lastInput;

    Console.Write($"Teléfono ({personToEdit.Phone}): ");
    var phoneInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(phoneInput) && IsValidPhone(phoneInput)) personToEdit.Phone = phoneInput;

    Console.Write($"Ciudad ({personToEdit.City}): ");
    var cityInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(cityInput)) personToEdit.City = cityInput;

    Console.Write($"Saldo ({personToEdit.Balance.ToString("N2", culture)}): ");
    var balanceInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(balanceInput) &&
        decimal.TryParse(balanceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal newBalance) &&
        newBalance >= 0)
    {
        personToEdit.Balance = newBalance;
    }

    Console.WriteLine("Persona actualizada correctamente.");

}

void AddPerson()
{

    int newId = people.Count + 1;

    Console.Write("Nombres: ");
    var name = Console.ReadLine();
    Console.Write("Apellidos: ");
    var lastName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName))
    {
        Console.WriteLine("Nombres y apellidos son obligatorios.");
        return;
    }

    Console.Write("Teléfono: ");
    var phone = Console.ReadLine();
    if (!IsValidPhone(phone))
    {
        Console.WriteLine("Número de teléfono inválido.");
        return;
    }

    Console.Write("Ciudad: ");
    var city = Console.ReadLine();

    Console.Write("Saldo: ");
    if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal balance) || balance < 0)
    {
        Console.WriteLine("Saldo inválido.");
        return;
    }

    var newPerson = new Person
    {
        Id = newId,
        Name = name ?? string.Empty,
        LastName = lastName ?? string.Empty,
        Phone = phone ?? string.Empty,
        City = city ?? string.Empty,
        Balance = balance
    };

    people.Add(newPerson);
    Console.WriteLine("Persona agregada exitosamente.");
}

bool IsValidPhone(string? phone)
{
    if (string.IsNullOrWhiteSpace(phone)) return false;
    var digits = new string(phone.Where(char.IsDigit).ToArray());
    return digits.Length >= 7;
}

void ShowPeople()
{


    foreach (var personItem in people)
    {
        Console.WriteLine("==============================");
        Console.Write(personItem.Id);
        Console.WriteLine($"  {personItem.Name} {personItem.LastName}");
        Console.WriteLine($"City: {personItem.City}");
        Console.WriteLine($"Phone: {personItem.Phone}");
        Console.WriteLine($"Balance: ${personItem.Balance.ToString("N2", culture)}");
        Console.WriteLine("==============================");
    }
}

string ShowMenu()
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("1. Mostrar personas");
    Console.WriteLine("2. Agregar persona");
    Console.WriteLine("3. Editar persona");
    Console.WriteLine("4. Eliminar persona");
    Console.WriteLine("5. Guardar archivo");
    Console.WriteLine("6. Reporte por ciudad");
    Console.WriteLine("0. Salir");
    Console.Write("Seleccione una opción: ");
    return Console.ReadLine() ?? "";
}


