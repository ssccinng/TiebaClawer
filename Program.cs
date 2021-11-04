using System.Text.RegularExpressions;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
// 用于记载我喜欢人物照片的爬虫

HttpClient http = new();
// var res = await http.GetAsync("https://tieba.baidu.com/p/3919405253");
// var res = await http.GetAsync("https://tieba.baidu.com/p/4893036169");
Regex reg = new(@"src=""(?<url>(https|http)://\S+?)""");
int idx = 0;

List<Task> jj = new();
// http.DefaultRequestHeaders
for (int i = 1; i <= 41; ++i) {
    // var res = await http.GetAsync($"https://tieba.baidu.com/p/4894341858?pn={i}");
    // var res = await http.GetAsync($"https://tieba.baidu.com/p/4891025277?pn={i}");
    var res = await http.GetAsync($"https://tieba.baidu.com/p/4883592773?pn={i}");
    // var res = await http.GetAsync($"https://tieba.baidu.com/p/7568114585?pn={i}");
    
    jj.Add(DownPage(await res.Content.ReadAsStringAsync()));
}
foreach (var item in jj)
{
    await item;
}
// var text = await res.Content.ReadAsStringAsync();
// File.WriteAllText("test.html", await res.Content.ReadAsStringAsync());
// Console.WriteLine("访问结束");





async Task DownPage(string text) {
    
    MatchCollection matches = reg.Matches(text);
    Console.WriteLine("开始读取第一页");
    List<Task<HttpResponseMessage>> gg =new();
    foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;
            if (groups["url"].Value.Contains("gss0") || groups["url"].Value.Contains("gsp0")){
                continue;
            }
            gg.Add(http.GetAsync(groups["url"].Value));
            
            
            // Console.WriteLine("'{0}' repeated at positions {1} and {2}",
            //                   groups["url"].Value,
            //                   groups[0].Index,
            //                   groups[1].Index);
        }
    foreach (var item in gg)
    {
        await File.WriteAllBytesAsync($"lili/{idx++}.jpg", await (await item).Content.ReadAsByteArrayAsync());
    }
        Console.WriteLine("读取完毕");
}