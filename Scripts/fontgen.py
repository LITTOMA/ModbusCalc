from fontTools import subset
from fontTools.ttLib import TTFont
import os

def walk_strings(dir, ext, encoding='utf-8'):
    for root, _, files in os.walk(dir):
        for file in files:
            if file.endswith(ext):
                with open(os.path.join(root, file), 'r', encoding=encoding) as f:
                    for line in f:
                        yield line.strip()

INPUT_FONT = 'SourceHanSansSC-Regular.otf'
OUTPUT_FONT = '../ModbusCalc/ModbusCalc/Assets/SourceHanSansSC-Regular-subset.otf'

strings = walk_strings('../ModbusCalc/ModbusCalc/Localizations', '.resx')
text = ''.join(sorted(set(''.join(strings))))
print(text)

options = subset.Options()
options.text = text

font = TTFont(INPUT_FONT)

subsetter = subset.Subsetter(options)
subsetter.populate(text=text)
subsetter.subset(font)

font.save(OUTPUT_FONT)
