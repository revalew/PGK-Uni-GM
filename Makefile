.PHONY: push-no-tag bump-version

push-no-tag:
	read -p "Enter commit message: " commitmsg; \
	git add .; \
	git commit -am "$$commitmsg"; \
	git push


bump-version:
	@echo "Current version: $(VERSION)"
	@read -p "Choose version type to bump (major, minor, patch -> 'major.minor.patch'): " versiontype; \
	case $$versiontype in \
		patch) newversion=$$(echo $(VERSION) | awk -F. '{printf "%d.%d.%d", $$1, $$2, $$3+1}');; \
		minor) newversion=$$(echo $(VERSION) | awk -F. '{printf "%d.%d.%d", $$1, $$2+1, 0}');; \
		major) newversion=$$(echo $(VERSION) | awk -F. '{printf "%d.%d.%d", $$1+1, 0, 0}');; \
		*) echo "Invalid option! Please choose either major, minor, or patch."; exit 1;; \
	esac; \
	sed -i "s/__version__ = \"$(VERSION)\"/__version__ = \"$$newversion\"/" $(VERSION_FILE); \
	echo "Version updated to $$newversion"; \
	read -p "Enter commit message: " commitmsg; \
	git add .; \
	git commit -am "$$commitmsg (version $$newversion)"; \
	git tag -fa v$$newversion -m "$$commitmsg"; \
	git push --follow-tags