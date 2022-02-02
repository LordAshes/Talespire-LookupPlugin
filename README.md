# Look Up Plugin

This unofficial TaleSpire allows the chat to be used to look up local and/or remote information using topic find or
keyword(s) find with the results of the search displayed in the chat.

This plugin, like all others, is free but if you want to donate, use: http://198.91.243.185/TalespireDonate/Donate.php

## Change Log

```
1.1.0: Added support for providers which can get data from websites
1.0.0: Initial release
```

## Install

Use R2ModMan or similar installer to install this plugin.

## Usage

The plugins adds 3 chat operations:

``/lu topic`` (Look Up): Displays the related topic. The topic given must match a topic file for the search to succeed.
``/fd keywords`` (Find): Display the first topic which contains the keyword or keywords.
``/fa keywords`` (Find All): Display all topics which contains the keyword or keywords.

Note: If multiple keywords are provdied then they must appear in the order given. For example "healing spell" will find
only references of "healing spell" and not just "healing" or just "spell" or even "spell of healing".

Note: Topic look ups must match exactly but they are not case sensitive. Find looks ups are case sensitive.

The find requests can only be used with local content. The lookup request, on the other hand, can use one or more provides
to look up the information. The first provider to have the information is used.

### Providers

Providers dictate how the look up function obtains data. Each provider is a different source of data and has its own set
of rules for extrating and formatting the data. The LookUp Plugin comes with two providers, a local files provider and
a DnD Beyond provider for spells.


### Demo Content

You can do a look up for Firebolt locally.
You can look up most 5E SRD spells using the sample D&D Beyond provider.

Note: Firebolt is one work in the local version but two words (Fire bolt) on DnD Beyond.

## How It Works

The LookUp plugin, when doing a lookup, calls the LookupEngine.exe which goes through the different providers, one by one,
until one of them provides content for the search. The LookUpEngine dumps the results to the console which the Lookup Plugin
grabs and writes to the Chat. You can test the LookUpEngine from the command line by running it with the search keys words
as the command line parameters. This can be useful when testing provider scripts.

## Creating Local Content

Currently, content must be placed in the ``CustomData/Sources`` sub-folder of this plugin. To create content files, create
a text file which has the name of the topic (and no extension). The contents of the file is the text that is to be
displayed when the topic is requested. While you can use core TS Chat supported formatting, it is recommended not to
use color formatting because the plugin uses color to highlight keywords for Find requests. As such during Find requested
the color formatting would be partially overwritten and probably look very odd.

## Creating Provider

Providers are defined using a special javascript file. Each provider, as source of information, has one file which indicates
the source (URL) of the provider, the space replacement character and provides the script to parse data from the provider
and make it correctly formatted for the char window.

A provider file is a javascript file with the two first lines being non-javascript.

The first line is always the keyword "URL: " (with a trailing space) and then the URL of the provider. The search word(s)
are represented by the place holder {search}. 

The second line is always the keyword "SPACE: " (with a trailing space) and indicates the character or characters that are
to replace spaces. For example, DnD Beyond replaces spaces with a dash in the URL.

The rest of the file is javascript content that determines how to parse data from the provider. This is useful to formatted
the results from the provider into a format compatible with the TS chat. The raw incoming data is stored in the variable
'content'. The script must return a string which is what will be displayed in the chat.

See the DndBeyond sample provider for the format of a provider file.

A number of helpers have been created to make parsing a wbesite easier:

#### Start()

This resets the buffer to the full contents obtained form the provider. Used at the beginning of individual data extractions.

#### FindSection(string)

This removes data from the buffer until it find the specified string. Use to find key sections in the provider data.

#### GetBetween(startString,endString)

Returns the text between the start string and the end string. White space, new lines and HTML tags are removed.

#### GetTextBetween(startString,endString)

Returns the text between the start string and the end string. White space and HTML tags are removed.


## Limitations

Currently the Find All look up will be useless in may cases because if the found topics are long, the results will exceed
the size of the core TS chat.

