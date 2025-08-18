UPDATE Players
        SET FirstName = @FirstName,
            LastName = @LastName,
            lastAttended = @College,
            guard = @isGuard,
            forward = @isForward,
            center = @isCenter,
            draftYear = @draftYear,
            draftRound = @draftRound,
            draftNumber = @draftNumber,
            height = @height,
            bodyWeight = @weight
        WHERE PersonId = @PlayerID