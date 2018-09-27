                 var interestValue = _db.Users.FirstOrDefault(x => x.Id == userId);

                bool isInterest = false;

                if (interestValue != null)
                {
                    if (interestValue.Interests != null)
                    {
                        isInterest = true;
                    }
                    else
                    {
                        isInterest = false;
                    }
                }