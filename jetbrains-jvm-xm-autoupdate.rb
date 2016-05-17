require 'yaml'

DIRS = YAML.load_file('config.yaml')['JETBRAINS_IDE_DIRECTORIES']['Linux']
DEF_XMS = '-Xms128m'
DEF_XMX = '-Xmx750m'

def update_jvm_xm_args(xms = ARGV[0], xmx = ARGV[1], ide_dirs = DIRS)
  ide_dirs.each{|_, dir| update_jvm_xm_arg(xms, xmx, dir)}
end

def update_jvm_xm_arg(xms, xmx, dir)
  vmoptions_file = vmoptions_filename(dir)
  file_updated = File.read(vmoptions_file).gsub(DEF_XMS, "-Xms#{xms}").gsub(DEF_XMX, "-Xmx#{xmx}")
  File.open(vmoptions_file, 'w') {|f| f.puts(file_updated)}
end

def vmoptions_filename(dir)
  vmoptions_file = Dir.entries("#{dir}/bin").select{|file| file.include?('64.vmoptions')}.first
  "#{dir}/bin/#{vmoptions_file}"
end


update_jvm_xm_args
