using System;
using System.Collections.Generic;

namespace SimpleCalculatorMVVM.About
{
    /// <summary>
    /// Информация о разработчиках и проекте
    /// </summary>
    public static class DeveloperInfo
    {
        // === Информация о продукте ===
        public static string ProductName => "Smart Calculator MVVM";
        public static string Version => "2.0.0";
        public static string Copyright => "Copyright © 2026";
        public static string Company => "SmartSoft Inc.";
        public static string Description => "Профессиональный калькулятор с архитектурой MVVM, поддержкой Undo/Redo, научными функциями и управлением памятью.";

        // === Информация о разработчиках ===
        public static IReadOnlyList<Developer> GetDevelopers() => new List<Developer>
        {
            new Developer
            {
                Name = "Иван Иванов",
                Role = "Архитектор и Team Lead",
                Email = "ivan.ivanov@smartsoft.com",
                GitHub = "@ivanov",
                Contributions = "Архитектура MVVM, паттерн Command, интеграция библиотек"
            },
            new Developer
            {
                Name = "Петр Петров",
                Role = "Разработчик UI",
                Email = "petr.petrov@smartsoft.com",
                GitHub = "@petrov",
                Contributions = "WPF интерфейс, стилизация, темы оформления"
            },
            new Developer
            {
                Name = "Мария Сидорова",
                Role = "Бэкенд разработчик",
                Email = "maria.sidorova@smartsoft.com",
                GitHub = "@sidorova",
                Contributions = "Математический движок, парсер выражений, научные функции"
            },
            new Developer
            {
                Name = "Алексей Смирнов",
                Role = "QA Инженер",
                Email = "alexey.smirnov@smartsoft.com",
                GitHub = "@smirnov",
                Contributions = "Тестирование, документация, сборка установщика"
            }
        };

        // === Лицензия ===
        public static string LicenseType => "MIT License";
        public static string LicenseText => @"
MIT License

Copyright (c) 2026 SmartSoft Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the 'Software'), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.";

        // === Системные требования ===
        public static IReadOnlyList<SystemRequirement> GetSystemRequirements() => new List<SystemRequirement>
        {
            new SystemRequirement { Name = "OS", Requirement = "Windows 10 версия 1809 или выше" },
            new SystemRequirement { Name = ".NET Runtime", Requirement = ".NET 6.0 или выше" },
            new SystemRequirement { Name = "RAM", Requirement = "512 MB (рекомендуется 1 GB)" },
            new SystemRequirement { Name = "Disk Space", Requirement = "50 MB" }
        };

        // === Сторонние библиотеки ===
        public static IReadOnlyList<ThirdPartyLibrary> GetThirdPartyLibraries() => new List<ThirdPartyLibrary>
        {
            new ThirdPartyLibrary { Name = "UnmanagedExports", Version = "1.2.7", License = "MIT" }
        };
    }

    /// <summary>
    /// Класс разработчика
    /// </summary>
    public class Developer
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string GitHub { get; set; }
        public string Contributions { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Role}";
        }
    }

    /// <summary>
    /// Системное требование
    /// </summary>
    public class SystemRequirement
    {
        public string Name { get; set; }
        public string Requirement { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Requirement}";
        }
    }

    /// <summary>
    /// Сторонняя библиотека
    /// </summary>
    public class ThirdPartyLibrary
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string License { get; set; }

        public override string ToString()
        {
            return $"{Name} v{Version} ({License})";
        }
    }
}