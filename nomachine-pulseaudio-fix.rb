require 'fileutils'

NX_LOG = '/usr/NX/var/log/nxserver.log'

def extract_nxdevice_dir_from_log
  log = File.readlines(NX_LOG)
  log.select{|line| line.include?('native.socket')}.last.split(' ').last.gsub('.', '')
end

uid = Process::UID.from_name(ARGV[0])
nx_audio_socket = extract_nxdevice_dir_from_log
pulseaudio_native_socket = "/run/user/#{uid}/pulse/native"
FileUtils.ln_s(pulseaudio_native_socket, nx_audio_socket)
