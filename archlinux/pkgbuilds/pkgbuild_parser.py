#!/usr/bin/env python3

# Adapted from https://gist.github.com/pyropeter/517956

# This is a parser for bash-style variable assignments.
# It fully (?) supports the syntax w/o braces, the ${}-syntax
# is implemented very hackish. (There are also detailed
# TODO-comments in various places)

# This is meant for Archlinux' AUR to be used to parse
# PKGBUILD's properly.

# At the moment I have problems to find PKGBUILD's that are challenging
# for this script.

import re
import sys

reName = re.compile(r"([\w_][\w\d_]*)")
reAssignment = re.compile(r"([\w_][\w\d_]*)=")


def bash_glob_to_regex(glob):
    # Reference: bash(1) "Pathname Expansion"
    # TODO:
    # * character classes, equivalence classes, that other foo
    #   ( [:upper:], [=c=], [.symbol.]
    #   First should be easy to implement, python's regexes even have
    #   similar classes (\w, \W, \d, \D, \s, \S)
    #   Afaik, the first syntax is used rarely, the following two are
    #   used _never_. No urge to implement them. 
    # * extended globs:
    #   ( ?(pattern), *(pattern), +(pattern), etc...)
    #   Never seen someone use these, no urge to implement them.
    # * bash may have different handling of \:
    #   This code just ignores the following characters' special meaning
    #   and lets it match itself. Bash may evaluate e.g. "\a" as "\a"
    #   and not as "a". (But both should eval "\*" as "*".)
    #   UPDATE: This should now be solved. But it was done with magic.

    res = ""
    ptr = 0
    while ptr < len(glob):
        if glob[ptr] == "\\":
            ptr += 1
            res += re.escape(glob[ptr])
        elif glob[ptr] == "*":
            res += ".*?"
        elif glob[ptr] == "?":
            res += "."
        elif glob[ptr] == "[":
            ptr += 1
            res += "["
            if glob[ptr] == "^" or glob[ptr] == "!":
                res += "^"
                ptr += 1
            if glob[ptr] == "]":
                res += "]"
                ptr += 1
            if glob[ptr - 1] in "[^!" and glob[ptr] == "\\":
                ptr += 1
            while glob[ptr] != "]":
                if glob[ptr] == "\\" and glob[ptr + 1] in "wsdbaWSDBAZ":
                    res += "\\"
                res += glob[ptr]
                ptr += 1
            res += "]"
        else:
            res += re.escape(glob[ptr])
        ptr += 1
    return res


def expand_params(symbols, text):
    # Reference: bash(1) "Parameter Expansion"
    # Done:
    # * ${foo}
    # * ${foo/glob/substitute}
    #
    # Not done:
    # * all other syntax
    # * support for arrays will not be needed
    # * support for 2nd-level indirection will not be needed
    #   ( foo="bar"; bar="baz"; ${!foo} -> "baz" )

    res = ""
    ptr = 0
    while ptr < len(text):
        if text[ptr] == "\\":
            ptr += 1
        elif text[ptr] == "$":
            ptr += 1

            # are there braces?
            if text[ptr] == "{":
                ptr += 1
                name = reName.match(text, ptr)
                ptr = name.end()

                if text[ptr] == "/":
                    # Pattern substitution
                    # ${parameter/pattern/string}

                    ptr += 1

                    # check if all occurrences should be substituted
                    sub_all = False
                    if text[ptr] == "/":
                        sub_all = True
                        ptr += 1

                    # fetch the pattern
                    pattern = ""
                    while text[ptr] != "/" and text[ptr] != "}":
                        if text[ptr] == "\\" and text[ptr + 1] in "\\}/":
                            ptr += 1
                        pattern += text[ptr]
                        ptr += 1

                    # fetch the substitute
                    substitute = ""
                    if text[ptr] != "}":  # there might be no substitute
                        ptr += 1
                        while text[ptr] != "}":
                            if text[ptr] == "\\":
                                ptr += 1
                            substitute += text[ptr]
                            ptr += 1

                    if pattern.startswith("#"):
                        align = 1
                        pattern = pattern[1:]
                    elif pattern.startswith("\\#"):
                        pattern = pattern[1:]
                    elif pattern.endswith("%%"):
                        align = 2
                        pattern = pattern[:-1]
                    elif pattern.endswith("\\%%"):
                        pattern = pattern[:-1]
                    else:
                        align = 0
                    regex = ((align == 1 and "^" or "") +
                             bash_glob_to_regex(pattern) +
                             (align == 2 and "$" or ""))

                    res += re.sub(regex, substitute,
                                  symbols.get(name.group(1), ""),
                                  0 if sub_all else 1)
                else:
                    # 'normal' expansion
                    res += symbols.get(name.group(1), "")
            else:
                name = reName.match(text, ptr)
                res += symbols.get(name.group(1), "")
                ptr = name.end()
        else:
            res += text[ptr]
        ptr += 1
    return res


def parse_str(symbols, line, ptr):
    # TODO:
    # * This does not parse escapes
    res = ""
    if line[ptr] == "'":
        ptr += 1
        while line[ptr] != "'":
            res += line[ptr]
            ptr += 1
        ptr += 1
    elif line[ptr] == '"':
        ptr += 1
        while line[ptr] != '"':
            res += line[ptr]
            ptr += 1
        res = expand_params(symbols, res)
        ptr += 1
    else:
        while (len(line) > ptr) and (not line[ptr] in " \t)"):
            res += line[ptr]
            ptr += 1
        res = expand_params(symbols, res)
    return ptr, res


def parse_file(filename):
    # TODO:
    # * Lines are not parsed like they should be.
    #   This is very unlikely to break the variable assignments
    #   at the beginning of the file, but will break if a 'string'
    #   spanning multiple lines is encountered and it uses \ at the end
    #   of the line (e.g. a bash script in a bash script)
    lines = [""]

    for line in filename:
        line = line[:-1]
        if not line:
            continue
        lines[-1] += line
        if line[-1] != "\\":
            lines.append("")

    symbols = {}
    i = 0
    while i < len(lines):
        line = lines[i]
        i += 1

        assignment = reAssignment.match(line)
        if not assignment:
            continue
        ptr = assignment.end()

        # the parser relies on proper syntax. syntax errors are caught here
        try:
            if line[ptr] == "(":
                symbols[assignment.group(1)] = []
                ptr += 1
                while True:
                    while (ptr < len(line)) and (line[ptr] != ")"):
                        if not line[ptr] in " \t":
                            ptr, val = parse_str(symbols, line, ptr)
                            symbols[assignment.group(1)].append(val)
                        else:
                            ptr += 1
                    if (ptr < len(line)) and (line[ptr] == ")"):
                        break
                    if len(lines) == i:
                        break
                    line = lines[i]
                    i += 1
                    ptr = 0
            else:
                ptr, symbols[assignment.group(1)] = parse_str(symbols, line, ptr)
        except IndexError as ie:
            print("Syntax error, continuing", file=sys.stderr)
            continue
    return symbols


if __name__ == '__main__':
    if len(sys.argv) < 2:
        print("Usage: %s <filename or '-'>" % (sys.argv[0]))
        exit(1)

    res = dict(parse_file(open(sys.argv[1]) if sys.argv[1] != "-" else sys.stdin))
    print("""%(pkgname)s %(pkgver)s
%(pkgdesc)s

Licenses: %(license)s
Architectures: %(arch)s

Dependencies: %(depends)s
    for make: %(makedepends)s

Source: %(source)s""" % res)
