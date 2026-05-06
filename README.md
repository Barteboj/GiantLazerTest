1. Instrukcja uruchomienia:
  1.1 Edytor:
    - wejdź na scenę StartMenu
    - kliknij Play
  1.2 Build:
    Projekt jest gotowy do buildownia na Windows. Wystarczy po prostu uruchomić buildowanie, a następnie uruchomić exe.
2. Decyzje architektoniczne
  - Stworzenie interfejsu IEvaluator, po to, żeby można było łatwo dodać nowe tryby oceny, ponieważ można się spodziewać nowych tym bardziej, że są już dwie (OCP, DIP)
  - Stworzenie interjesu walidacji ILayoutValidator, żeby łatwo było dodać nowe. Tutaj już z samego opisu zadania wiadomo o co chodzi (OCP, DIP)
  - Stworzenie interfejsów ILayoutValidationProcessController, ILibraryItem, ILibraryItemPrefabsContainer, ILayoutSaveController, żeby zmniejszyć zależności między modułami, zmieniać sposób działania bez ingerencji w istniejący kod (OCP, DIP)
  - Rozdzielenie logiki zapisywania i wczystywania layoutu od logiki związanego z tym ui, żeby zmniejszyć zależności, żeby łatwo było zmienić sposób zapisu / odczytu bez ingerencji w logikę UI i vice versa (SRP, OCP)
  - Rozdzielenie logiki procesu walidacji od logiki związanego z tym ui, żeby zmniejszyć zależności, łatwo było zmienić sam proces bez ingerencji w logikę ui i vice versa (SRP, OCP)
  - Rozdzielenie logiki samego elementu biblioteki od logiki interakcji z nim (przesuwanie, context menu...), żeby zmniejszyć zależności (SRP, OCP)
  - Stworzenie osobnej klasy DeskController do przyczepiania elementów do stołu, zarządzania przyczepionymi elementami w ten sposób mamy jedno wejście gdzie elementy są dodawne, usuwane i gdzie można uzyskać informacje o obecnych elementach
  - Definicje konkretnych elementów biblioteki tworzone za pomocą prefabów ze względu na prostotę i elastyczność rozwiązania (KISS)
  - Stworzenie scriptable objectu LibraryItemPrefabsContainer, żeby łatwo było zmieniać dostępne elementy z poziomu projektu i łatwo się dostawać do tych danych
  - Podział na moduły, żeby zmniejszyć zależności, posegregować code base i przyspieszyć kompilację
  - Zastosowanie nowego Input System i SerializeActionReference w miejscach gdzie potrzebny był input, żeby oddzielić logikę sterowania od pozostałej logiki.
  - Zastosowanie eventów gdy elemtny są dodawane/usuwane ze stołu, żeby rozlużnić zależności, wyizolować to czym się klasa zajmuje (SRP), łatwe dołączanie nowych funkcjonalności do wykonania w tym momencie bez modyfikacji istniejącego kodu (OCP)
4. Co bym jeszcze zrobił gdybym miał więcej czasu
  - Zastosowałbym DependencyInjection żeby zmniejszyć ilość ręcznie wpinanych zależności po hierarchii, nieraz powtarzające się. Również, żeby pozbyć się tego jednego nieszczęsnego "singletona" GameController
  - Zastosowałbym EventBus zamiast static event w niektórych miejscach, żeby zmniejszyć zależności
  - Wprowadziłbym id do konkretncyh instancji elementów, żeby walidacja działała również gdy elementy danego typu się powtarzają
  - Nie wiedziałem do końca jak mieliście na myśli tę interakcję myszka + klawiatura. Jakbym miał więcej czasu to pewnie bym to co mam jeszcze przerobił na formę bardziej podobną do VR, np. zastosowałbym ThirdPersonController
  - Dodałbym fabrykę do tworzenia elementów biblioteki, bo teraz się trochę powtarza kod między panelem biblioteki i panelem wczytywania, bo w obu miejscach tworzymy elementy biblioteki (DRY). Może zrobiłbym do tego, że obiekty elementów biblioteki tworzą się na podstawie definicji w scriptable object zamiast prefabów, ale nie jestem pewien czy warto, bo jest wtedy mniejsza elastyczność, ale za to łatwiej by było komuś nieobeznanemu w Unity stworzyć nowe elementy
5. Tipy
  - Aby usunąć połączenie pomiędzy portami należy nacisnąć prawym przyciskiem myszy na jeden z dwóch połączonych portów.
