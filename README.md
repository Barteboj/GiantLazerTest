### 1. Instrukcja uruchomienia:
*   **1.1 Edytor:**
    *   pobierz wersję projektu z release tag Test
    *   uruchom projekt za pomocą Unity 6.3.10f1
    *   wejdź na scenę StartMenu
    *   kliknij Play
*   **1.2 Build:**
    *   Pobierz build z release tag Test lub wykonaj build z dołączonej tam wersji projektu. Projekt jest gotowy do buildownia na Windows.
    *   Uruchom plik .exe

### 2. Decyzje architektoniczne
*   Stworzenie interfejsu **IEvaluator**, po to, żeby można było łatwo dodać nowe tryby oceny. Można się spodziewać nowych, bo są już dwie (**OCP, DIP**)
*   Stworzenie interjesu walidacji **ILayoutValidator**, żeby łatwo było dodać nowe. Tutaj już z samego opisu zadania wiadomo o co chodzi (**OCP, DIP**)
*   Stworzenie interfejsów **ILayoutValidationProcessController, ILibraryItem, ILibraryItemPrefabsContainer, ILayoutSaveController**, żeby zmniejszyć zależności między modułami, zmieniać sposób działania bez ingerencji w istniejący kod (**OCP, DIP**)
*   Rozdzielenie logiki zapisywania i wczystywania layoutu od logiki związanego z tym ui, żeby zmniejszyć zależności. Dzięki temu łatwo będzie zmienić sposób zapisu / odczytu bez ingerencji w logikę UI i vice versa (**SRP, OCP**)
*   Rozdzielenie logiki procesu walidacji od logiki związanego z tym ui, żeby zmniejszyć zależności. Dzięki temu łatwo będzie zmienić sam proces bez ingerencji w logikę ui i vice versa (**SRP, OCP**)
*   Rozdzielenie logiki samego elementu biblioteki od logiki interakcji z nim (przesuwanie, context menu...), żeby zmniejszyć zależności (**SRP, OCP**)
*   Stworzenie klasy **DeskController** do przyczepiania elementów do stołu i zarządzania przyczepionymi elementami. W ten sposób mamy jedno wejście gdzie elementy są dodawne, usuwane i gdzie można uzyskać informacje o obecnych elementach
*   Definicje konkretnych elementów biblioteki tworzone za pomocą prefabów ze względu na prostotę i elastyczność rozwiązania (**KISS**)
*   Stworzenie scriptable objectu **LibraryItemPrefabsContainer**, aby ułatwić zarządzanie dostępnymi elementami z poziomu projektu oraz usprawnić dostęp do tych danych.
*   Zastosowanie eventów gdy elementy są dodawane/usuwane ze stołu, żeby rozlużnić zależności, wyizolować to czym się klasa zajmuje (**SRP**) oraz ułatwić dołączanie nowych funkcjonalności do wykonania w tym momencie bez modyfikacji istniejącego kodu (**OCP**)
*   Stworzenie klasy PortVisualData wiążącego typ portu z jego wyglądem, żeby łatwo można było dodawać nowe konfiguracje bez konieczności ingerencji w kod (**OCP**)
*   Zastosowanie wzorca DTO dla systemów zapisu i odczytu layoutu (LibraryItemDTO, PortDTO). Implementacja ta służy jako kontrakt danych, izolując wewnętrzną logikę klas MonoBehaviour od formatu serializacji (JSON). Dzięki temu struktura plików zapisu jest niezależna od zmian w implementacji klas silnika gry, co zapobiega błędom przy deserializacji i pozwala na wyodrębnienie wyłącznie danych niezbędnych do rekonstrukcji stanu obiektu.
*   Stworzenie atrybutu **RequireInterface** wraz z dedykowanym PropertyDrawer, umożliwiającego jawną serializację interfejsów w inspektorze Unity. Pozwala to na realizację zasady DIP przy zachowaniu wygody pracy w edytorze. Rozwiązanie to gwarantuje bezpieczeństwo typów i, w przeciwieństwie do alternatywnych obejść, jest zgodne z zasadą **DRY** – nie wymaga pisania powtarzalnego kodu walidującego dla każdego pola
*   Podział na moduły w celu uporządkowania zależności, odseparowania warstw bazy kodu oraz przyspieszenia procesu kompilacji.
*   Zastosowanie namespaces, aby odzwierciedlić modularną budowę projektu. Pozwoliło to na uniknięcie konfliktów nazw. Struktura namespaces ściśle koreluje z podziałem na moduły (Assembly Definitions), wymuszając czytelną hierarchię zależności
*   Zastosowanie jawnego przypisywania wartości do elementów Enum. Dzięki temu dodawanie, usuwanie lub zmiana kolejności elementów wewnątrz enuma nie powoduje błędnego mapowania danych w istniejących assetach i prefabach Unity.
*   Zastosowanie nowego **Input System** i **InputActionReference** w miejscach, gdzie potrzebny był input, aby oddzielić logikę sterowania od pozostałej logiki. Dzięki temu możliwa jest łatwa zmiana mapowania klawiszy bez ingerencji w kod oraz natywne wsparcie dla wielu kontrolerów przy zachowaniu czystości skryptów gameplayowych.

### 3. Co bym jeszcze zrobił gdybym miał więcej czasu
*   Zastosowałbym **Dependency Injection** w celu dalszej redukcji bezpośrednich zależności i wyeliminowania ręcznego przypisywania referencji w inspektorze. Pozwoliłoby to również na usunięcie klasy typu Singleton (GameController), co poprawiłoby modularność systemu.
*   Zastosowałbym **EventBus** zamiast static event w niektórych miejscach, żeby zmniejszyć zależności
*   Zastosowałbym wzorzec **Object Pooling** dla często tworzonych i usuwanych elementów biblioteki oraz innych obiektów, aby zminimalizować narzut procesora (Instantiate/Destroy) oraz zmniejszyć narzut Garbage Collectora.
*   Wprowadziłbym ID do konkretnych instancji elementów, żeby umożliwić poprawne działanie walidacji, gdy elementy danego typu się powtarzają
*   W przypadku niejasności w specyfikacji dotyczącej interakcji myszy i klawiatury, zdecydowałem się na implementację obecnego rozwiązania. Dysponując większą ilością czasu, rozwinąłbym system w stronę doświadczeń zbliżonych do VR, wprowadzając np. **ThirdPersonController**
*   Wprowadziłbym wzorzec Fabryki do tworzenia elementów biblioteki, aby wyeliminować powielanie logiki (zgodnie z zasadą DRY) między panelem wyboru a systemem wczytywania. Rozważałem również migrację na system budowania obiektów w oparciu o dane ze Scriptable Objects zamiast gotowych prefabów. Choć mogłoby to nieznacznie ograniczyć elastyczność wizualną, znacząco uprościłoby proces dodawania nowych elementów przez osoby nietechniczne

### 4. Tipy
*   Aby usunąć połączenie pomiędzy portami należy nacisnąć prawym przyciskiem myszy na jeden z dwóch połączonych portów.
