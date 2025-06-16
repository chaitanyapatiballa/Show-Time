using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class movietheater
{
    public int movieid { get; set; }
    public Movie Movie { get; set; } = null!;

    public int theaterid { get; set; }
    public Theater Theater { get; set; }

}
