# Lab 1 - Tracer
## Инструкция 
- Скомпилировать приложение
- Отдельно собрать (build) сборки **Tracer.Serialization.Json**, **Tracer.Serialization.Xml** и **Tracer.Serialization.Yaml**. Они автоматически не компилируются с приложением, так как нигде не используются, ведь информацию о них мы получаем рефлексией (по условию)
- в **".\Tracer\Tracer.Example\bin\Debug\net8.0"** создать папки **Plugins** и **Tracers**
- В папку **Plugins** скопировать 4 **.dll** файла
  - Tracer.Serialization.Json.dll (**".\Tracer\Tracer.Serialization.Json\bin\Debug\net8.0\Tracer.Serialization.Json.dll"**)
  - Tracer.Serialization.Xml.dll (**".\Tracer\Tracer.Serialization.Xml\bin\Debug\net8.0\Tracer.Serialization.Xml.dll"**)
  - Tracer.Serialization.Yaml.dll (**".\Tracer\Tracer.Serialization.Yaml\bin\Debug\net8.0\Tracer.Serialization.Yaml.dll"**)
  - YamlDotNet.dll - это библиотека нужна для сериализации в Yaml файл. Она хранится глубоко на диске **C** (в моём случае **"C:\Users\arefi\.nuget\packages\yamldotnet\16.3.0\lib\net8.0\YamlDotNet.dll"**), поэтому есть более удобный способ её найти и скопировать:
    - открыть проект в IDE, к примеру JetBrains Rider
    - Перейти в папку (решение) **Serialization**
    - Выбрать проект **Tracer.Serialization.Yaml**
    - Кликнуть на Dependencies -> .NET 8.0 -> Packages -> YamlDotNet/16.3.0
    - Кликнуть по файлу YamlDotNet правой кнопкой мыши, нажать "Open in" -> "Explorer"
    - И Вас сразу перекинет к этому файлу в проводнике
- Всё готово. Осталось запустить и увидеть результат в ранее созданной папке **Tracers**

# Lab 2 - Faker
