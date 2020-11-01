# cw2

## Autor: *Paweł Rutkowski s18277*

### Dodanie nowego sposobu zapisu danych
Dodanie nowego sposobu danych wymaga jedynie dodanie nowej implementacji interfejsu `Cw2.Serializers.IObjectSerializer`
i dodanie obsługi przekazanego parametru do metody `Cw2.ArgumentParser.DetermineCorrectSerializer` i zawartej w niej
konstrukcji `switch`.

Dzięki zastosowaniu interfejsów i typów generycznych nie wymagane są żadne inne działania.

### Dodanie nowego rodzaju danych do odczytu
Aby przejść na typ odczytu TSV (Tab Separated Values), lub inny format przechowujący pola rozdzielone wybranym
znakiem/sekwencją znaków wystarczy zamienić delimiter przekazany do konstruktora obiektu `Cw2.Parsers.LineDelimitedParser`.

Aby odczytać całkiem inny rodzaj pliku należy utworzyć nową implementację interfejsu `Cw2.Parsers.IObjectParser`
i przekazać ten obiekt do konstruktora obiektu `Cw2.DataConverter`.

### Zmiana odczytywanych obiektów
Odczyt nowego rodzaju obiektów wymaga dodania nowej klasy przechowującą dane tych obiektów (która jest dodatkowo
oznaczona atrybutem `Serializable`) i dodanie nowej implementacji interfejsu `Cw2.Parsers.IObjectParser`, która będzie
parsować plik wejściowy do obiektów.

Jeżeli dane dalej są zapisane w formacie CSV (lub podobnym) wystarczy implementacja interfejsu
`Cw2.Parsers.Fields.IFieldsToObjectParser`, dalej można skorzystać z parsera `Cw2.Parsers.LineDelimitedParser`.

Pozostałe klasy (w tym `Cw2.ArgumentParser`, `Cw2.DataConverter` i `Cw2.Serializers.IObjectSerializer`) są stworzone
w sposób generyczny, więc ich modyfikacja nie będzie konieczna.