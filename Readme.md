# Lombiq Antispam Readme



## Project Description

An Orchard module for better spam protection.


## Lombiq Antispam - Registration

After enabling the feature there will be a new content type, Registration Spam Protector. An ad-hoc content item of this type will be displayed on the registration page and updated after registration; when it has a validaiton error it will prevent registration.

You can add any anti-spam content part (like JavaScriptAntiSpamPart from the Orchard.AntiSpam module) to this content type and the registration form will use it to filter out spambots.


## JavaScript Antispam Part

**Note that JavaScript Antispam Part is as of 01.09.2013 part of the built-in Orchard Antispam module in the latest source code and thus is included in the Orchard 1.7.1 release.**

After installation you'll see a new content part, "JavaScript Antispam Part". Just add this part to any content type whose form you want to protect from being submitted by clients that don't have JS enabled. Since spambots usually don't run JS this will protect those forms from being spammed.

This works equally well with e.g. comments and forms created with Custom Forms.


## reCAPTCHA v3

After enabling the feature the built-in Orchard.AntiSpam reCAPTCHA v2 feature will be overriden with reCAPTCHA v3.

The API keys can be set on the Settings / Spam page. Keys can be acquired from [https://www.google.com/recaptcha](https://www.google.com/recaptcha) and make sure you register your site using reCAPTCHA v3.

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-antispam](https://bitbucket.org/Lombiq/orchard-antispam) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Antispam](https://github.com/Lombiq/Orchard-Antispam) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.