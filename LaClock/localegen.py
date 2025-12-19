#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Auto translate and update locale.

Author: HomeOnMars

-------------------------------------------------------------------------------
"""

from os.path import sep
import json
import translators

class LocaleGen:
    TARGETS_DEFAULT: dict[str, str] = {
        # filename: language codename
        'de-DE': 'de',
        'en-US': 'en',
        'eo-EO': 'eo',
        'es-ES': 'es',
        'fr-FR': 'fr',
        'it-IT': 'it',
        'ja-JP': 'ja',
        'ko-KR': 'ko',
        'pl-PL': 'pl',
        'pt-BR': 'pt',
        'ru-RU': 'ru',
        'zh-HANS': 'zh',
        'zh-HANT': 'zh-TW',
    }

    def __init__(self, dirpath: str = ""):
        self.dirpath: str = dirpath
        self.src: dict[str, str] = {}
        self.srcfilename: str = ""
        self.srclangname: str = "auto"

    def __repr__(self):
        # return f"{self.dirpath = }\n{self.src = }"
        src_prettyprint_list = [
            f'{entry}: {txt}' for entry, txt in self.src.items()]
        if len(src_prettyprint_list) > 128:
            src_prettyprint_list = src_prettyprint_list[:127]
            src_prettyprint_list.append("...")
        src_prettyprint = '\n\t' + '\n\t'.join(src_prettyprint_list)
        return f"self.dirpath = '{self.dirpath}'\nself.srclangname = '{self.srclangname}'\nself.src = {src_prettyprint}"

    def load(self, filename: str = 'en-US', langname: None|str = None):
        if langname is not None:
            self.srclangname = langname
        elif filename in self.TARGETS_DEFAULT:
            self.srclangname = self.TARGETS_DEFAULT[filename]
        else:
            self.srclangname = "auto"
        
        self.srcfilename = filename

        filepath = f"{self.dirpath}{filename}.json"
        with open(filepath, 'r') as f:
            self.src = json.load(f)
        return self

    def update_files(
        self,
        targets: dict[str, str] = TARGETS_DEFAULT,
        prefix_tag: str = "(AutoTranslated)",
        translator: None|str = None,
        remove_empty_entries: bool = False,
        verbose: bool = True,
        **kwargs,
    ):
        """Update locale files in targets. Skips existing entries in each files.
        
        Must set translator param to make it actually do the translation.
            See translators docs at <https://github.com/UlionTse/translators>.
        kwargs will be passed to translators python library.
        """

        for filename, langname in targets.items():

            if filename == self.srcfilename:
                continue

            # load target locale
            target = {}
            filepath = f"{self.dirpath}{filename}.json"
            if verbose: print(filepath)
            try:
                with open(filepath, 'r') as f:
                    target = json.load(f)
            except FileNotFoundError:
                if verbose:
                    print("\tFile not found. Will create if necessary.")

            # update target locale
            count = 0
            if remove_empty_entries:
                for entry, txt in target.items:
                    if not txt:
                        del target[entry]
                        count += 1
            if count and verbose:
                print(f"\tRemoved {count} entries that are empty.")

            missing_locale_entries = set(self.src) - set(target)
            extra_locale_entries = set(target) - set(self.src)
            if extra_locale_entries:
                if verbose:
                    extra_locale_entries_txt = '\n\t\t'.join(list(extra_locale_entries))
                    print(f"\t** Warning: There are {len(extra_locale_entries)} extra entries in '{filepath}' file:\n\t\t{extra_locale_entries_txt}")
            if not missing_locale_entries:
                if verbose: print("\tSkipped.")
                # continue

            if verbose:
                total_char = sum([
                    len(self.src[entry]) for entry in missing_locale_entries])
                print(
                    f"\tUpdating {len(missing_locale_entries)} entries (Total {total_char} characters)...",
                    end=' ')
            for entry in missing_locale_entries:
                if translator is not None:
                    target[entry] = prefix_tag + translators.translate_text(
                        self.src[entry],
                        translator=translator,
                        from_language=self.srclangname,
                        to_language=langname,
                        **kwargs)
                else:
                    if not remove_empty_entries:
                        target[entry] = ""
            if verbose: print("Done.")

            # sort target entry like src
            target = {entry: target[entry] for entry in self.src if entry in target} | target

            # dump new target locale
            if verbose: print("\tOverwriting...", end=' ')
            with open(filepath, 'w', encoding='utf8') as f:
                json.dump(target, f, indent=2, ensure_ascii=False)
            if verbose: print("Done.")

        return self


if __name__ == '__main__':
    # _ = translators.preaccelerate_and_speedtest()    # optional
    _ = LocaleGen(f"lang{sep}").load().update_files(translator="google", prefix_tag="")
