# mp3-to-podcast-rss-generator
Super simple program for converting a folder of local mp3s into an RSS feed. Currently ONLY seems to work with podcast addict - for some reason google podcasts hates this. Set a webserver to point to the MP3 folder and subscribe to the podcast you're hosting. Huzzah!


# to use this program

1. find the full physical file path of the folder where you keep your Audiobook MP3s edit the value in `mvc\appsettings.json` `PODCAST_FILE_PATH` to be the full file path of that folder
2. Find your computer's network name for the rest of this guide this will be referred to as `NETWORK_NAME`
2. ensure you have dotnet 6 installed https://dotnet.microsoft.com/en-us/download
3. from the project root folder run the command line `dotnet run --urls *:80 --project mvc/mvc.csproj` to start the webserver
4. browse your list of books on the url `http://[NETWORK_NAME]`
5. select the book you want to listen to
6. in find the file `podcast.rss.xml` filename and copy the link to that file
7. paste that use into your podcast app subscription
8. select download the files within your podcast app
9. stop the webserver with `CTRL+C`


# Notes

Tested and working in:

    - podcast addict
    - ios podcast app

Not working in:

    - google podcasts

- currently works on HTTP only (probably because of signing)
- you can move it to whatever port you would like by changing the `--urls parameter`
- if you change the port or URL you will need to regenerate your RSS files using the `?reindex` command load your website with `http://[NETWORK_NAME]?reindex` and it will regenerate all podcast feed files
- the expects a JPG IMAGE file which is allowed to have a variable name. This gets copied to a file with the name of `tile.jpg` things go wrong if there is only one jpg named tile.jpg. So like - make sure you have a file named "cover.jpg" or something just to be safe. Though it all works with broken images too so don't worry too much.
- this was created just for my own personal use but a couple friends were complaining about having the same problem with audiobooks so I did a little tweaking. Feel free to open an issue, send a PR, fork or whatever.
