# Look Up Plugin

This unofficial TaleSpire allows the chat to be used to look up local information using topics find or keyword(s) find
with the results of the search displayed in the chat.

This plugin, like all others, is free but if you want to donate, use: http://198.91.243.185/TalespireDonate/Donate.php

## Change Log

```
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

### Demo Content

You can do a look up for Firebolt or Healing Word.

## Creating Content

Currently, content must be placed in the ``CustomData/Sources`` sub-folder of this plugin. To create content files, create
a text file which has the name of the topic (and no extension). The contents of the file is the text that is to be
displayed when the topic is requested. While you can use core TS Chat supported formatting, it is recommended not to
use color formatting because the plugin uses color to highlight keywords for Find requests. As such during Find requested
the color formatting would be partially overwritten and probably look very odd.

## Limitations

Currently the Find All look up will be useless in may cases because if the found topics are long, the results will exceed
the size of the core TS chat.

