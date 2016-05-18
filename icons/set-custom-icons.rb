require 'yaml'

CUSTOM_ICONS = YAML.load_file('custom-icons.yaml')
APPLICATIONS_BASEDIR = "/usr/share/applications"
CUSTOM_ICONS_BASEDIR = "mac-osx"
ICON_BASEDIR = '/home/nick/Code/misc-tools/icons'

def set_custom_icons
  CUSTOM_ICONS.keys.each{|dest| set_custom_icon(dest)}
end

def set_custom_icon(filepath)
  filename = APPLICATIONS_BASEDIR + "/" + filepath
  file = File.readlines(filename)
  puts file
  puts "---------------------"
  file.map! do |line|
    if line.include?('Icon')
      "Icon=#{get_custom_icon_filepath(filepath)}\n"
    else
      line
    end
  end
  newfile = file.join('')
  puts newfile
  File.open(filename, 'w') {|f| f.puts(newfile)}
  puts "Wrote #{filename}"
end

def get_custom_icon_filepath(short_filepath)
  ICON_BASEDIR + '/' + CUSTOM_ICONS_BASEDIR + "/" + CUSTOM_ICONS[short_filepath]
end


set_custom_icons