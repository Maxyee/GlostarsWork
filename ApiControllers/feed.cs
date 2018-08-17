                var size = model.MutualFollowerPictures.Count();
                var data = new ArrayList();
                var k = 0;
                competionPicCount = model.CompetitionPictures.Count();
                for (var i = 0; i < size; i++)
                {
                    data.Add(model.MutualFollowerPictures.Skip(i).First());
                    if (i != 0 && (i + 1) % 5 == 0)
                    {
                        if (competionPicCount >= k)
                        {
                            if (model.CompetitionPictures.Skip(k).Any())
                            {
                                data.Add(model.CompetitionPictures.Skip(k).First());
                                k++;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                }