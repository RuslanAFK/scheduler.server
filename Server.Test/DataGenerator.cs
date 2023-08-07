using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Server.Core.Models;
using Server.Persistence;

namespace Server.Test;

public static class DataGenerator
{
    public static User CreateTestUser(int id = 0, string name = "", string password = "")
    {
        return new User()
        {
            Id = id,
            Username = name,
            Password = password
        };
    }

    public static IFormFile CreateTestFormFile(string fileName = "", int length = 0)
    {
        var baseStream = A.Dummy<Stream>();
        var streamOffset = A.Dummy<int>();
        var name = A.Dummy<string>();
        return new FormFile(baseStream, streamOffset, length, name, fileName);
    }

    public static string GetPrivateKey()
    {
        return "MIIEpQIBAAKCAQEA2DzwpiJTEdh9hvI9QJIcyI92SBYW/KOlvsDAZsAdDU" +
               "k03wtPPVFnHOVwm+rtM79hH59iq9PxeIdaJKHI5HRjFdRb/GABH5/Z6i8+Kq7p" +
               "KDS2VERq3ACSMNuCJ65ke1bJDVEenKys+hMKDb2Ip3nusGF49YXXg6LbWm3ma7t\n" +
               "J5oeM8OoBXLH6uCKsVt9pOYvAZDB0TWXPRfCk5uskN3gxAN6cjBRhDsn4Vrheodp" +
               "gsZ+cVlV4zPEPjP/Ca/SY2kQjBCyIBmaPf/m535lDoMP2jnwqOpRhXcqrWTUCOij1" +
               "4T2P4b74sH9Rx9aRnA5/a4H9kEeklDKGHRWCrBK6tmUgeQIDAQABAo\nIBAQCiQjA" +
               "t6cm9tV6UGUd/IWS51nTiKLk9ACtKFOcK8xOZuZoT2C+wilm+ZCh4xvMRBoWBrh7j" +
               "YtlqIN6yaDgPvYnwgnY3zW5qZY+mW6bhbniEc/FxEBnDViZcxQpIbmL17ixVcs5usF" +
               "/oEstTfiqByUwjTDDww2rxWw4QMDFcG6Cbey9HzgqrE\nt0w+Wu43j8AYZtKSxjW+h" +
               "3tE0xgjUsv3DNQdYjBLZ0Z4/lMsB+MHEz6e/1/TJowyGNuwF4V0N51MrgXHg9pi7UjQ" +
               "ZVJ8Gr765C9KsUErme2tZvos74KCtYzIMSOY1MVeslIPG1JJhoPQ3xRhjsGv1S5Vf0tl" +
               "9RTfJUFAoGBANmTPftbMwp2ZxWu\nffCgn+xvEqBW9cXdGrL1+q7BJzaICX2EgX6zD8L" +
               "2JKDLPQiIXj52Rx85lrmi6ZpXRM125ghgnxf/utr+u8GtCPxzi0DONr4LUmgS8D73+0" +
               "zPYPFkEVnDIvVN3lPCys+jOw8baYnrPdJIq/T3TGhbngCUAUiDAoGBAP5tPuZAJLz7Spx" +
               "wk3JDk3a\nRaPYAksZZT6rSZjUlJwezkFezPrTyTStSVl2ELZ1B4N527T073PSekZbkrh" +
               "s+NrS3tUd3JOZ6yIQY+//rYeItgoVQt7gZ2wokRMb8x9bbKVmUc36y1WODzPlzgEgyKkn" +
               "hIUPXb995XUUAycHWTIpTAoGBAMA0YfH12/4nIOO3dQwoaX6tlK/Ogm\nmb7KUhxaWxfl" +
               "mfDXsznk32EztwxGTDhhROm6rkQ+oirrMpZuJwq5gyq/3ElWbXBBPIKsdqe+DAlcjXIub" +
               "6C39jE5cc7IQrQwGcG/PG/c/kTT6DezS4h0jON4qeJMvqZPYPrREXtlneZc/PAoGBAJY" +
               "T1uB2wbT//fjdpvvlxJxSFbnWiL2bfRTkW\nwnKSoWOc/xnbPvLWZ3OSceL6mQysfRH7" +
               "pUKNMHOr050wgar4hUjsDjhnNCfaJwTKMLDE9AYzD7baCOejMgksLU38qFYUcHXgXEhC" +
               "CJVYplaejcb8Dn4JGkiMYl+y3eiiWBfinKlAoGAZcs4bMUCpWVwZG1muxZj44BZw3Zt0v" +
               "BSrVq0x0tugAzk\nhiE2F9KfjYsuPvGLBpUl2KPqtP8OuqcpJbhvzTujW/GMFgkTJ6ziJ" +
               "2bJXFBWjacIXvXDeSxLJY5f/7Za154QtCS0Y1yZBW4sRNbxxyxPRXuAQF3gQ4LHffFE99c" +
               "u3+0=";
    }
    public static DbContextOptions<SchedulerDbContext> CreateNewInMemoryDatabase()
    {
        return new DbContextOptionsBuilder<SchedulerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    public static Subject CreateTestSubject(int id, string username)
    {
        return new Subject
        {
            Count = 0,
            Hours = 0,
            Id = id,
            Minutes = 0,
            Name = "",
            Type = 0,
            User = new User()
            {
                Id = 0,
                Password = "",
                Username = username
            }
        };
    }
}