Using ordinary static method from *Utils is preferable over extension method
because it's easier to find usages of methods using CTRL+F search.
It may be useful if these methods are used in *.vm, *.cshtml, *.aspx files.
However, if method is used too often, consider to convert it to extension method for convenience

If you have extension method for too specific use-case, it will be annoing to see it every time,
especially if this method has too abstract and confusing name,
for example, method ToFormattedString() - with this method name I assume that it may format value to absolutely anything