require 'fileutils'

NX_LOG = '/usr/NX/var/log/nxserver.log'

def extract_nxdevice_dir_from_log
  log = File.readlines(NX_LOG)
  native_socket_filepath = log.select{|line| line.include?('native.socket')}.last.split(' ').last
  native_socket_filepath[0..native_socket_filepath.size - 2]
end

uid = Process::UID.from_name(ARGV[0])
nx_audio_socket = extract_nxdevice_dir_from_log
pulseaudio_native_socket = "/run/user/#{uid}/pulse/native"
FileUtils.ln_s(pulseaudio_native_socket, nx_audio_socket)
