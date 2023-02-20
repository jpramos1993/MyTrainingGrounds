
string text = "Nick ";

string trimmed = text.TrimEnd(); // Created a new string

var nick1 = "Nick";
var nick2 = "Nick";
var result = nick1 == nick2; // True only one is created in the heap because they are equal interns in compiler

{
    var nick = "Nick";
    var chapsas = "Chapsas";
    var nickChapsas = nick + chapsas;
    var result1 = nickChapsas == "NickChapsas"; // True
    var result2 = ReferenceEquals(nickChapsas, "NickChapsas"); // False
}
{
    var nick = "Nick";
    var chapsas = "Chapsas";
    var nickChapsas = string.Intern(nick + chapsas);
    var result1 = nickChapsas == "NickChapsas"; // True
    var result2 = ReferenceEquals(nickChapsas, "NickChapsas"); // True

}

Console.WriteLine();