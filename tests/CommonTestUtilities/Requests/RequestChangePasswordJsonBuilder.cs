﻿using Bogus;
using Store.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int passwordLength = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(u => u.CurrentPassword, (f) => f.Internet.Password())
            .RuleFor(u => u.NewPassword, (f) => f.Internet.Password(passwordLength));
    }
}
